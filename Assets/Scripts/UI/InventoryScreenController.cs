using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScreenController : InventoryUIManager
{
    [SerializeField] private GameObject _itemInfoPanel;

    [SerializeField] private Button _itemActionButton;
    [SerializeField] private Text _itemActionButtonText;
    [SerializeField] private string _itemActionButtonTextValue;
    [SerializeField] private Text _itemName;
    [SerializeField] private Image _itemImage;

    public void FillInfoPanel(Item item, Slot slot)
    {
        _itemName.text = item.GetItemName();
        _itemImage.sprite = item.GetItemIcon();
        ConfigureActionButton(item, slot);
    }

    public void ShowInfoPanel()
    {
        _itemInfoPanel.SetActive(true);
    }

    public void HideInfoPanel()
    {
        _itemInfoPanel.SetActive(false);
    }

    public void RemoveActionButtonListeners()
    {
        _itemActionButton.onClick.RemoveAllListeners();
    }

    public override void SetCellsListeners()
    {
        foreach (InventoryUICell cell in _inventoryCellsList)
        {
            Item _item = Engine.Instance.GetItemByID(cell.GetItemID());
            Slot _slot = Engine.Instance.GetItemSlotByID(cell.GetItemID());

            cell.GetCellButton().onClick.AddListener(RemoveActionButtonListeners);
            cell.GetCellButton().onClick.AddListener(ShowInfoPanel);
            cell.GetCellButton().onClick.AddListener(delegate { FillInfoPanel(_item, _slot); });
            cell.GetCellButton().onClick.AddListener(delegate { UpdateActionButton(_slot); });
        }
    }

    public void ConfigureActionButton(Item item, Slot slot)
    {
        if (item.GetType() == typeof(Eatable)) //если айтем предназначен для хила (съедания)
        {
            Eatable eatable = (Eatable)item;
            if (Engine.Instance._currentGameMode == GameMode.Battle)
            {
                //
            }
            else
            {
                _itemActionButton.onClick.AddListener(delegate { Engine.Instance.Player.RestoreHealth(eatable.GetHealthToRestore()); });
                _itemActionButton.onClick.AddListener(delegate { Engine.Instance.RemoveItemFromInventory(slot._itemId, 1); });
                _itemActionButton.onClick.AddListener(delegate { UpdateActionButton(slot); });

                _itemActionButtonText.text = _itemActionButtonTextValue;
            }

        }
        else
        {
            Debug.LogError("Incorrect type");
        }
        _itemActionButton.onClick.AddListener(ClearCells);
        _itemActionButton.onClick.AddListener(FillCells);
        _itemActionButton.onClick.AddListener(SetCellsListeners);
    }

    public void UpdateActionButton(Slot slot)
    {
        if (slot._quantity == 1)
        {
            _itemActionButton.onClick.AddListener(HideInfoPanel);
        }
    }

}
