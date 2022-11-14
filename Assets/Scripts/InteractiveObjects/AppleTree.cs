using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTree : InteractiveObject
{
    private Animator _anim;
    [SerializeField] private Apple[] _apples;

    internal override void Start()
    {
        _interactive = true;
        _anim = GetComponentInChildren<Animator>();
    }

    internal override void OnInteract()
    {
        if(_interactive)
        {
            _anim.Play("Impact");
            DropApples();
            _interactive = false;
        }
    }

    private void DropApples()
    {
        foreach (Apple _apple in _apples)
        {
            _apple.Drop();
        }
    }
}
