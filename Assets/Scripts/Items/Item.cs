using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : ScriptableObject
{
    [SerializeField] private Sprite _itemIcon;
    [SerializeField] private float _itemWeight;
}
