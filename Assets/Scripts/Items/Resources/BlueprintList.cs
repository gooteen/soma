using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Blueprint
{
    public ItemID _itemToBeCrafted;
    public List<Slot> _resourcesNeeded;

    public bool ItemCanBeCrafted()
    {
        foreach (Slot resourceNeeded in _resourcesNeeded)
        {
           Slot _playerResource = Engine.Instance.GetItemSlotByID(resourceNeeded._itemId);
            if (_playerResource == null)
            {
                return false;
            }
            if (_playerResource._quantity < resourceNeeded._quantity)
            {
                return false;
            }
        }
        return true;
    }

    public void Craft()
    {
        Engine.Instance.AddItemToInventory(_itemToBeCrafted, 1);
        foreach (Slot resourceNeeded in _resourcesNeeded)
        {
            Engine.Instance.RemoveItemFromInventory(resourceNeeded._itemId, resourceNeeded._quantity);
        }
    }
}

[CreateAssetMenu(fileName = "BlueprintList", menuName = "BlueprintList")]
public class BlueprintList : ScriptableObject
{
    public Blueprint[] _blueprints;
}
