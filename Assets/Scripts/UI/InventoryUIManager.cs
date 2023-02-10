using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class InventoryUIManager : MonoBehaviour
{
    [SerializeField] internal GameObject _inventoryCellPrefab;
    [SerializeField] internal List<InventoryUICell> _inventoryCellsList; 

    [SerializeField] internal Transform _content;

    public virtual void FillCells()
    {
        List<Slot> _items = Engine.Instance.GetItemsInInvetory();
        foreach (Slot slot in _items)
        {
            GameObject _itemSlot = Instantiate(_inventoryCellPrefab, _content);
            InventoryUICell _cell = _itemSlot.GetComponent<InventoryUICell>();
            _inventoryCellsList.Add(_cell);
            Item item = Engine.Instance.GetItemByID(slot._itemId);

            _cell.SetItemID(slot._itemId);
            _cell.SetCellImage(item.GetItemIcon());
            _cell.SetCellQuantity(slot._quantity);
        }
    }

    public abstract void SetCellsListeners();
   

    public virtual void ClearCells() 
    {
        foreach (InventoryUICell cell in _inventoryCellsList)
        {
            Destroy(cell.gameObject);
        }
        _inventoryCellsList.Clear();
    }

    
}
