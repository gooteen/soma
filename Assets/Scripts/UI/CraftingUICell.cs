using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingUICell : InventoryUICell, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData data)
    {
        Debug.Log("HOVERING");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
