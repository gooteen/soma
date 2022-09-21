using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffCombatEnemyController : MonoBehaviour
{
    [SerializeField] private CombatInitiator _initiator;
    [SerializeField] private CharacterAnimation _animation;
    [SerializeField] private Vector2 _dirAtStart;

    void Start()
    {
        SetEnemyStartDirection();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Initiated" + gameObject);
        _initiator.InitializeFight();
    }

    public void SetEnemyStartDirection()
    {
        _animation.SetStaticDirection(_dirAtStart);
    }
}
