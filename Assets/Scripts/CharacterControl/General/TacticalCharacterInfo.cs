using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TacticalCharacterInfo : MonoBehaviour
{
    [SerializeField] internal float _healthPoints = 50;
    [SerializeField] internal int _actionPoints = 5;

    //TEMP
    [SerializeField] internal int _damage = 5;
    [SerializeField] internal OverlayTile _activeTile;

    public void TakeDamage(float damage)
    {
        _healthPoints -= damage;
    }

    public void TakeAwayActionPoints(int points)
    {
        _actionPoints -= points;
    }

    public float GetHP()
    {
        return _healthPoints;
    }

    public int GetAP()
    {
        return _actionPoints;
    }

    public void SetActiveTile(OverlayTile tile)
    {
        _activeTile = tile;
        // блокируем плитку на ход противкника
    }

    public OverlayTile GetActiveTile()
    {
        return _activeTile;
    }
}
