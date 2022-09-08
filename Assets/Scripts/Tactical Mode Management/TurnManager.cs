using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _charactersInBattle;
    [SerializeField] private GameObject _currentCharacter;

    [SerializeField] private float _timeBetweenTurns;

    private int index;
    
    void Start()
    {
        index = 0;
    }

    public void ToNextTurn()
    {
        SetNextCharacter();
        StartCoroutine("Cooldown");
    }

    public GameObject[] GetCharactersInBattle()
    {
        return _charactersInBattle;
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

    private IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(_timeBetweenTurns);
        if (_currentCharacter.tag == "EnemyTactical")
        {
            CallEnemyMove(_currentCharacter);
        }
    }

    private void SetNextCharacter()
    {
        index++;
        if (index == _charactersInBattle.Length)
        {
            index = 0;
        }

        _currentCharacter = _charactersInBattle[index];
    }

    private void CallEnemyMove(GameObject enemy) 
    {
        enemy.GetComponent<TacticalEnemyAI>().StartTurn();
    }
}
