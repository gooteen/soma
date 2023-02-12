using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//bring back later
//public enum MagicMode { FilledRange, FourLines, SingleLine };

[CreateAssetMenu(fileName = "Magic Ability", menuName = "Magic Ability")]
public class MagicAbility : ScriptableObject
{
    [SerializeField] private bool _unlocked;

    // Change to a unique enum type later

    [SerializeField] private WeaponMode _magicMode;
    [SerializeField] private int _damage;
    [SerializeField] private int _hitCost;
    [SerializeField] private int _reach;

    public bool Unlocked { get { return _unlocked; } }
    public int HitCost { get { return _hitCost; } }
    public void OnChosen()
    {
        // need to change the CursorController for this to work
        CursorController.Instance.SetCombatRange(_magicMode, _reach);
        Engine.Instance.ChangeTacticalMode(TacticalMode.Magic);
        Engine.Instance.TacticalPlayer.SetCurrentMagicAbility(this);
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
}
