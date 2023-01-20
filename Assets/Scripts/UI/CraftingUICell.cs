using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingUICell : InventoryUICell, IPointerEnterHandler, IPointerExitHandler
{
    public bool _isHoveredOver;
    public bool _isOccupied;
    public void OnPointerEnter(PointerEventData data)
    {
        Debug.Log("HOVERING");
        _isHoveredOver = true;
    }

    public void OnPointerExit(PointerEventData data)
    {
        Debug.Log("stopped HOVERING");
        _isHoveredOver = false;
    }

    void Start()
    {
        _isHoveredOver = false;
        _isOccupied = false;
    }

    public void SetOccupied()
    {
        _isOccupied = true;
    }

    public void SetUnoccupied()
    {
        _isOccupied = false;
    }
}
