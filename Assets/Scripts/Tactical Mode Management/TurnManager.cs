using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _charactersInBattle;
    [SerializeField] private GameObject _currentCharacter;

    [SerializeField] private float _timeBetweenTurns;

    [SerializeField] private int index;
    
    void Start()
    {
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
        if (index == _charactersInBattle.Count)
        {
            index = 0;
        }

        Debug.Log("Count: " + _charactersInBattle.Count + " Index: " + index);

        _currentCharacter = _charactersInBattle[index];
        Debug.Log("CurrentCharacter: " + _currentCharacter);

        _currentCharacter.GetComponent<TacticalCharacterInfo>().RefillActionPoints();
        LockCharacterTiles();

        if (_currentCharacter.tag != "EnemyTactical")
        {
            Engine.Instance.UpdatePlayerGateway(_currentCharacter.GetComponent<TacticalPlayerGateway>());
            CursorController.Instance.ShowCursor();
            CursorController.Instance.SetInRangeTiles();
        }
        else
        {
            CursorController.Instance.HideCursor();
            Engine.Instance.UpdatePlayerGateway(null);
        }
    }

    private IEnumerator CoolDown()
    {
        SetNextCharacter();
        Debug.Log("Coroutine one");
        yield return new WaitForSeconds(_timeBetweenTurns);
        Debug.Log("Coroutine two");

        if (_currentCharacter.tag == "EnemyTactical")
        {
            Debug.Log("YO");
            CallEnemyMove(_currentCharacter);
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
}
