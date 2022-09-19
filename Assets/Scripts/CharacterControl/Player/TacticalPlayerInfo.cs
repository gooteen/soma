using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalPlayerInfo : TacticalCharacterInfo
{
    [SerializeField] private HeroStats _stats;
    public override void Initialize()
    {
        base._healthPoints = _stats._healthPoints;
        base._maxActionPoints = _stats._maxActionPoints;
        base._damage = _stats._damage;
    }
}
