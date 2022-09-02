using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TacticalCharacterInfo : MonoBehaviour
{
    [SerializeField] internal float _healthPoints = 50;
    [SerializeField] internal float _actionPoints = 5;
    [SerializeField] internal OverlayTile _activeTile;

    public void TakeDamage(float damage)
    {
        _healthPoints -= damage;
    }

    public float GetHP()
    {
        return _healthPoints;
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
