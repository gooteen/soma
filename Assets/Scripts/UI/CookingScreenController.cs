using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingScreenController : InventoryUIManager
{
    void Start()
    {
        
    }

    //temporary
    private void OnDisable()
    {
        ClearCells();
    }

    private void OnEnable()
    {
        FillCells();
        SetCellsListeners();
    }

    void Update()
    {
        
    }

    public override void SetCellsListeners()
    {
        foreach (InventoryUICell cell in _inventoryCellsList)
        {
            cell.GetCellButton().onClick.AddListener(delegate { Debug.Log("I'm working!"); });
        }
    }
}
