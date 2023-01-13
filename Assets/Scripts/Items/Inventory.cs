using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Slot
{
    public ItemID _itemId;
    public int _quantity;

    public Slot(ItemID id, int quantity)
    {
        _itemId = id;
        _quantity = quantity;
    }
    public void IncreaseQuantity(int quantity)
    {
        _quantity += quantity;
    }

    public void DecreaseQuantity(int quantity)
    {
        _quantity -= quantity;
        if (_quantity < 0)
        {
            _quantity = 0;
        } 
    }
}

[CreateAssetMenu(fileName = "Inventory", menuName = "Inventory")]
public class Inventory : ScriptableObject
{
    public List<Slot> _items;
    public ItemMapSO _itemMap;

    public static Inventory Instance { get; set; }
    
    public void AddItem(ItemID id, int quantity)
    {
        bool _itemFound = false;
        
        for (int i = 0; i < _items.Count; i++)
        {
            if(_items[i]._itemId == id)
            {
                _itemFound = true;
                _items[i].IncreaseQuantity(quantity);
                return;
            }
        }
        if(!_itemFound)
        {
            Slot _slot = new Slot(id, quantity);
            _items.Add(_slot);
        }
    }

    public void RemoveItem(ItemID id, int quantity)
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i]._itemId == id)
            {
                _items[i].DecreaseQuantity(quantity);
                if (_items[i]._quantity == 0)
                {
                    _items.RemoveAt(i);
                }
            }
        }
    }

    public IdToItem FindItemInMap(ItemID id) 
    {
        Debug.Log("item id within FindItemInMa method: " + id);
        for (int i = 0; i <= _itemMap._items.Length; i++)
        {
            if (_itemMap._items[i]._id == id)
            {
                return _itemMap._items[i];
            } 
        }
        return null;
    }
}
