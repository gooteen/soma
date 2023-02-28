using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Blueprint
{
    public ItemID _itemToBeCrafted;
}

[CreateAssetMenu(fileName = "BlueprintList", menuName = "BlueprintList")]
public class BlueprintList : ScriptableObject
{
    
}
