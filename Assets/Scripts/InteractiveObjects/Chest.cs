using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : InteractiveObject
{
    [SerializeField] private Slot[] _items;

    internal override void Start()
    {
        _interactive = true;
    }

    internal override void OnInteract()
    {
        if (_interactive)
        {
            Debug.Log("Heeey!");
            _interactive = false;
            foreach (Slot item in _items)
            {
                Engine.Instance.AddItemToInventory(item._itemId, item._quantity);
            }
        } 
    }
}
