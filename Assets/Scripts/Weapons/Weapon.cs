using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponMode { FilledRange, FourLines, SingleLine };

public class Weapon : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private int _hitCost;

    //replace with an array of MagiAbility instances later
    [SerializeField] private int[] _abilities;

    [SerializeField] private WeaponMode _mode;
    [SerializeField] private int _reach;

    public void OnChosen()
    {
        CursorController.Instance.SetCombatRange(_mode, _reach);
        Engine.Instance.ChangeTacticalMode(TacticalMode.Combat);
    }
       
    public void OnUsed()
    {
        TacticalCharacterInfo _focusedEnemy = CursorController.Instance.GetCurrentFocusedEnemy();
        if (_focusedEnemy != null)
        {
            _focusedEnemy.TakeDamage(_damage);
            Engine.Instance.TacticalPlayer.TakeAwayActionPoints(_hitCost);
        }
    }

    public int GetHitCost()
    {
        return _hitCost;
    }
}
