using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalPlayerInfo : TacticalCharacterInfo
{
    [SerializeField] private HeroStats _stats;
    [SerializeField] private Weapon _weapon;
    [SerializeField] private MagicAbility _currentMagicAbility;

    public MagicAbility MagicAbility
    {
        get
        {
            return _currentMagicAbility;
        }
        set
        {
            _currentMagicAbility = value;
        }
    }

    public override void Initialize()
    {
        _maxHealthPoints = _stats._healthPoints;
        _maxActionPoints = _stats._maxActionPoints;
        _currentActionPoints = _maxActionPoints;
        _currentHealthPoints = _maxHealthPoints;
        _damage = _stats._damage;
        _weapon = _stats.CurrentWeapon;
    }

    public void OnWeaponChosen()
    {
        if (_weapon != null)
        {
            _weapon.OnChosen();
        }
    }


    public List<MagicAbility> GetAbilitiesOnWeapon()
    {
        return _weapon.GetMagicAbilities();
    }

    public void OnWeaponUsed()
    {
        if (_weapon != null)
        {
            _weapon.OnUsed();
        }
    }

    public int GetHitCost()
    {
        if (_weapon != null)
        {
           return _weapon.GetHitCost();
        } else
        {
            return 0;
        }
    }
}
