using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemSprinkler))]
public class AppleTree : InteractiveObject
{
    [SerializeField] private Slot _itemSlot;
    private Animator _anim;
    private ItemSprinkler _sprinkler;

    internal override void Start()
    {
        _interactive = true;
        _sprinkler = GetComponent<ItemSprinkler>();
        _anim = GetComponentInChildren<Animator>();
    }

    internal override void OnInteract()
    {
        if(_interactive)
        {
            _anim.Play("Impact");
            _interactive = false;
        }
        _sprinkler.StartSpawningItems(new Slot[] { _itemSlot });
    }
}
