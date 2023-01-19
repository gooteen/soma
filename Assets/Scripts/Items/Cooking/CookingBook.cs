using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum CookingMethod { Raw, Fryed, Baked };

[System.Serializable]
public class Recipe
{
    public bool _isUnlocked;

    public List<ItemID> _itemsNeeded; // items needed to cook the _itemToCook (in the given order)
    public ItemID _itemToCook;
    public CookingMethod _method;

    public Slot TryCooking(List<Slot> itemsInSlots, CookingMethod method)
    {
        for (int i = 0; i <= itemsInSlots.Count; i++)
        {
            if (itemsInSlots[i]._itemId != _itemsNeeded[i])
            {
                return new Slot(ItemID.Null, 0);
            }
        }

        if(_method == method)
        {
            return new Slot(_itemToCook, CalculateQuantity(itemsInSlots));
        } else
        {
            return new Slot(ItemID.Null, 0);
        }
    }

    public int CalculateQuantity(List<Slot> itemsInSlots)
    {
        List<int> _slotQuantities = new List<int>();

        for (int i = 0; i <= itemsInSlots.Count; i++)
        {
            if (itemsInSlots[i]._itemId != ItemID.Null)
            {
                _slotQuantities.Add(itemsInSlots[i]._quantity);
            }
        }
        return _slotQuantities.Min();
    }
}

[CreateAssetMenu(fileName = "Cooking Book", menuName = "CookingBook")]
public class CookingBook : ScriptableObject
{
    public Recipe[] _recipes;
}
