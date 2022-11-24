using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _charactersInBattle;
    [SerializeField] private List<GameObject> _deadCharacters;
    [SerializeField] private GameObject _currentCharacter;

    [SerializeField] private float _timeBetweenTurns;

    [SerializeField] private int index;
    
    void Start()
    {
        index = -1;
    }

    public void ClearCharactersList()
    {
        _charactersInBattle.Clear();
        _deadCharacters.Clear();
        index = -1;
    }

    public void ToNextTurn()
    {
        StartCoroutine("CoolDown");
    }

    public void AddCharacterToTheList(GameObject character)
    {
        _charactersInBattle.Add(character);
    }

    public List<GameObject> GetCharactersInBattle()
    {
        return _charactersInBattle;
    }

    public List<GameObject> GetDeadCharactersInBattle()
    {
        return _deadCharacters;
    }

    public GameObject GetCurrentCharacter()
    {
        return _currentCharacter;
    }


    public GameObject FindCharacterByTile(OverlayTile tile)
    {
        foreach (GameObject character in _charactersInBattle)
        {
            if (character.GetComponent<TacticalCharacterInfo>().GetActiveTile() == tile)
            {
                return character;
            }
        }
        return null;
    }

    public void SetNextCharacter()
    {
        index++;

        if (index >= _charactersInBattle.Count)
        {
            index = 0;
        }

        UIManager.Instance.ManageQueueMarkers(index);
        Debug.Log("Count: " + _charactersInBattle.Count + " Index: " + index);

        _currentCharacter = _charactersInBattle[index];
        Debug.Log("CurrentCharacter: " + _currentCharacter);

        _currentCharacter.GetComponent<TacticalCharacterInfo>().RefillActionPoints();
        LockCharacterTiles();
        ManageCharacterMarkers();

        if (_currentCharacter.tag != "EnemyTactical")
        {
            Engine.Instance.UpdatePlayerGateway(_currentCharacter.GetComponent<TacticalPlayerGateway>());
            CursorController.Instance.ShowCursor();
            CursorController.Instance.SetMovementRange();
            UIManager.Instance._toCombatModeButton.onClick.AddListener(delegate { Engine.Instance.TacticalPlayer.OnWeaponChosen(); } );
        }
        else
        {
            CursorController.Instance.HideCursor();
            Engine.Instance.UpdatePlayerGateway(null);
        }
    }

    public bool EnemiesBeaten()
    {
        int enemiesInTheList = 0;
        int slainEnemiesInTheList = 0;
        foreach (GameObject character in _charactersInBattle)
        {
            if (character.tag == "EnemyTactical")
            {
                enemiesInTheList += 1;
                if (character.GetComponent<TacticalCharacterInfo>().IsDead())
                {
                    slainEnemiesInTheList += 1;
                }
            }
        }
        if (enemiesInTheList == slainEnemiesInTheList)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool PlayersBeaten()
    {
        int playersInTheList = 0;
        int slainPlayersInTheList = 0;
        foreach (GameObject character in _charactersInBattle)
        {
            if (character.tag == "PlayerTactical")
            {
                playersInTheList += 1;
                if (character.GetComponent<TacticalCharacterInfo>().IsDead())
                {
                    slainPlayersInTheList += 1;
                }
            }
        }
        if (playersInTheList == slainPlayersInTheList)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator CoolDown()
    {
        for (int i = 0; i < _charactersInBattle.Count; i++)
        {
            if (_charactersInBattle[i].GetComponent<TacticalCharacterInfo>().IsDead())
            {
                Debug.Log("Dead??");
                _deadCharacters.Add(_charactersInBattle[i]);
                _charactersInBattle.RemoveAt(i);
                UIManager.Instance.RemoveCombatantCellAt(i);
            }
        }
        if (EnemiesBeaten() || PlayersBeaten())
        {
            // экран проигрыша
            Debug.Log("her)");
            //UIManager.Instance.ShowTransitionPanel();
            //MapManager.Instance.ClearArena();
        }
        else
        {
            SetNextCharacter();
            yield return new WaitForSeconds(_timeBetweenTurns);

            if (_currentCharacter != null)
            {
                if (_currentCharacter.tag == "EnemyTactical")
                {
                    CallEnemyMove(_currentCharacter);
                }
            }
        }
    }

    private void CallEnemyMove(GameObject enemy) 
    {
        enemy.GetComponent<TacticalEnemyAI>().StartTurn();
    }

    private void LockCharacterTiles()
    {
        for (int i = 0; i < _charactersInBattle.Count; i++)
        {
            Debug.Log("Index: " +i);
            if (i == index)
            {
                Debug.Log("Unlocked tile for " + i);
                _charactersInBattle[i].GetComponent<TacticalCharacterInfo>().GetActiveTile().UnlockTile();
            } else
            {
                Debug.Log("Locked tile for " + i);
                _charactersInBattle[i].GetComponent<TacticalCharacterInfo>().GetActiveTile().LockTile();
            }
        }
    }

    private void ManageCharacterMarkers()
    {
        for (int i = 0; i < _charactersInBattle.Count; i++)
        {
            if (_currentCharacter == _charactersInBattle[i])
            {
                _charactersInBattle[i].GetComponent<TacticalCharacterInfo>().ShowMarker();
            } else
            {
                _charactersInBattle[i].GetComponent<TacticalCharacterInfo>().HideMarker();
            }
        }
    }
}
