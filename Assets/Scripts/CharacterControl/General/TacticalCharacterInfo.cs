using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] private int _canvasSortingOrder;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject _marker;
    [SerializeField] private Image _healthBar;

    void Awake()
    {
        _currentActionPoints = _maxActionPoints;
        _currentHealthPoints = _maxHealthPoints;
        _canvas.sortingOrder = _canvasSortingOrder;
    }

    private void Update()
    {
        _healthBar.fillAmount = GetHealthRatio();
    }

    public virtual void ShowMarker()
    {
        _marker.SetActive(true);
    }

    public virtual void HideMarker()
    {
        _marker.SetActive(false);
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
