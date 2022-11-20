using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TacticalCharacterInfo : MonoBehaviour
{
    [SerializeField] internal float _maxHealthPoints;
    [SerializeField] internal int _maxActionPoints;
    [SerializeField] internal int _currentActionPoints;
    [SerializeField] internal float _currentHealthPoints;

    //PROBABLY TEMP
    [SerializeField] internal string _nameInBattle;

    //TEMP
    [SerializeField] internal int _damage = 5;
    [SerializeField] internal OverlayTile _activeTile;

    void Awake()
    {
        _currentActionPoints = _maxActionPoints;
        _currentHealthPoints = _maxHealthPoints;
    }

    public virtual string GetNameInBattle()
    {
        return _nameInBattle;
    }

    public virtual void Initialize()
    {
        //
    }

    public virtual float GetHealthRatio()
    {
        return _currentHealthPoints / _maxHealthPoints;
    }

    public virtual void TakeDamage(float damage)
    {
        _currentHealthPoints -= damage;
    }

    public virtual void TakeAwayActionPoints(int points)
    {
        _currentActionPoints -= points;
    }

    public virtual void RefillActionPoints()
    {
        _currentActionPoints = _maxActionPoints;
    }

    public float GetHealthPoints()
    {
        return _currentHealthPoints;
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
