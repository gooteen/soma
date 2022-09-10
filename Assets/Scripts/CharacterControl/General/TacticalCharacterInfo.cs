using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TacticalCharacterInfo : MonoBehaviour
{
    [SerializeField] internal float _healthPoints = 50;
    [SerializeField] internal int _maxActionPoints = 20;
    [SerializeField] internal int _currentActionPoints;

    //TEMP
    [SerializeField] internal int _damage = 5;
    [SerializeField] internal OverlayTile _activeTile;

    void Awake()
    {
        _currentActionPoints = _maxActionPoints;
    }

    public void TakeDamage(float damage)
    {
        _healthPoints -= damage;
    }

    public void TakeAwayActionPoints(int points)
    {
        _currentActionPoints -= points;
    }

    public void RefillActionPoints()
    {
        _currentActionPoints = _maxActionPoints;
    }

    public float GetHealthPoints()
    {
        return _healthPoints;
    }

    public int GetActionPoints()
    {
        return _currentActionPoints;
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
