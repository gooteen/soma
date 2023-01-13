using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : ScriptableObject
{
    [SerializeField] private string _itemName;
    [SerializeField] private Sprite _itemIcon;
    [SerializeField] private float _itemWeight;

    public string GetItemName()
    {
        return _itemName;
    }

    public Sprite GetItemIcon()
    {
        return _itemIcon;
    }

    public float GetItemWeight()
    {
        return _itemWeight;
    }
}
