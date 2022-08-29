using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TacticalCharacterInfo : MonoBehaviour
{
    [SerializeField] private float _healthPoints = 50;
    [SerializeField] private float _actionPoints = 5;
    [SerializeField] private OverlayTile _activeTile;

    public void TakeDamage(float damage)
    {
        _healthPoints -= damage;
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
