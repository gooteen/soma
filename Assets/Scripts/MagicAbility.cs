using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MagicMode { FilledRange, FourLines, SingleLine };

[CreateAssetMenu(fileName = "Magic Ability", menuName = "Magic Ability")]
public class MagicAbility : ScriptableObject
{
    [SerializeField] private bool _unlocked;
    [SerializeField] private MagicMode _magicMode;
    [SerializeField] private int _damage;
    [SerializeField] private int _hitCost;
    [SerializeField] private int _reach;
}
