using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemID { Wheat, Apple, LapisBerry, Flour, Egg, ScrambledEggs, 
    BerryJam, AppleJam, ApplePie, BerryPie, PanCakes, Pita, 
    CaterpillarMeat, RegularSword, IronOre, IronIngot, SilverOre, SilverIngot, CopperOre, CopperIngot, Null }

[System.Serializable]
public class IdToItem
{
    public ItemID _id;
    public Item _item;
}

[CreateAssetMenu(fileName = "ItemMap", menuName = "ItemMap")]
public class ItemMapSO : ScriptableObject
{
    public IdToItem[] _items;
}
