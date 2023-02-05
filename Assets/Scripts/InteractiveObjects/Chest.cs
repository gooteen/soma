using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemSprinkler))]
public class Chest : InteractiveObject
{
    [SerializeField] private Slot[] _items;
    private ItemSprinkler _sprinkler;

    internal override void Start()
    {
        _interactive = true;
        _sprinkler = GetComponent<ItemSprinkler>();
    }

    internal override void OnInteract()
    {
        if (_interactive)
        {
            Debug.Log("Heeey!");
            _interactive = false;
            _sprinkler.StartSpawningItems(_items);

        }
    }
}
