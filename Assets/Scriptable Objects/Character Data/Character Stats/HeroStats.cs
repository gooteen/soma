using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName="Hero Stats", menuName="Hero Stats")]

public class HeroStats : ScriptableObject
{
    [SerializeField] public string _name;
    [SerializeField] public float _healthPoints = 50;
    [SerializeField] public int _maxActionPoints = 20;
    [SerializeField] public int _damage = 5;
    [SerializeField] private Weapon _currentWeapon;
    [SerializeField] private Weapon _defaultWeapon;

    public Weapon CurrentWeapon { 
        get 
        {
            if (_currentWeapon == null)
            {
                return _defaultWeapon;
            } else
            {
                return _currentWeapon;
            }
        } 
    }

    public void SetCurrentWeapon(Weapon weapon)
    {
        // set _currentWeapon to barehanded if current weapon is unequipped

        if (weapon == null)
        {
            _currentWeapon = _defaultWeapon;
        } else
        {
            _currentWeapon = weapon;
            weapon.Equip();
        }
    }
}

