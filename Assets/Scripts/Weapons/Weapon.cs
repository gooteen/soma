using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private int _hitCost;

    //replace with an array of MagiAbility instances later
    [SerializeField] private int[] _abilities;

    public abstract void OnChosen();   
    public abstract void OnUsed();   
}
