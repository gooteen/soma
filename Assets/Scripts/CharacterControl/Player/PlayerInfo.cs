using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private float _healthPoints;
    [SerializeField] private HeroStats _stats;

    void Awake()
    {
        _healthPoints = _stats._healthPoints;
    }

    public void RestoreHealth(float healthToRestore)
    {
        _healthPoints += healthToRestore;
    }
}
