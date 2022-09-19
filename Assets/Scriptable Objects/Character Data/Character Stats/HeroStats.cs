using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName="Hero Stats", menuName="Hero Stats")]

public class HeroStats : ScriptableObject
{
    [SerializeField] public float _healthPoints = 50;
    [SerializeField] public int _maxActionPoints = 20;
    [SerializeField] public int _damage = 5;
}

