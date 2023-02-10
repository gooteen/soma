using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScreenController : InventoryUIManager
{
    [SerializeField] private GameObject _itemInfoPanel;

    [SerializeField] private Button _itemActionButton;
    [SerializeField] private Text _itemActionButtonText;
    [SerializeField] private Text _itemName;
    [SerializeField] private Image _itemImage;
    [SerializeField] private Image _equippedWeaponImage;
    [SerializeField] private Button _unequipWeaponButton;

    public override void FillCells()
    {
        base.FillCells();

        
        List<Slot> _items = Engine.Instance.GetItemsInInvetory();

        foreach (Slot slot in _items)
        {
            Item _item = Engine.Instance.GetItemByID(slot._itemId);
            if (_item.GetType() == typeof(Weapon))
            {
                Weapon _weapon = (Weapon)_item;
                if(_weapon.Equipped)
                {
                    UpdateUnequipWeaponButton(_weapon);
                    UpdateEquippedWeaponImage(_weapon.GetItemIcon());
                }
            }
        }
    }

    public override void ClearCells()
    {
        base.ClearCells();
        UpdateEquippedWeaponImage(null);
    }

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
            cell.GetCellButton().onClick.AddListener(delegate { UpdateActionButton(_slot._quantity, Engine.Instance.GetItemByID(_slot._itemId)); });
        }
    }

    public void ConfigureActionButton(Item item, Slot slot)
    {
        _itemActionButton.gameObject.SetActive(true);

        if (item.GetType() == typeof(Eatable)) // если айтем предназначен для хила (съедания)
        {
            Eatable _eatable = (Eatable)item;
            if (Engine.Instance._currentGameMode == GameMode.Battle)
            {
                //
            }
            else
            {
                _itemActionButton.onClick.AddListener(delegate { Engine.Instance.Player.RestoreHealth(_eatable.GetHealthToRestore()); });
                _itemActionButton.onClick.AddListener(delegate { Engine.Instance.RemoveItemFromInventory(slot._itemId, 1); });
                Item _itemTemp = Engine.Instance.GetItemByID(slot._itemId);
                _itemActionButton.onClick.AddListener(delegate { UpdateActionButton(slot._quantity, _itemTemp); }); 

                _itemActionButtonText.text = _eatable.GetActionText();
            }
        }
        else if (item.GetType() == typeof(Weapon)) // если айтем является оружием
        {
            Weapon _weapon = (Weapon)item;
            if (Engine.Instance._currentGameMode == GameMode.Battle)
            {
                //
            }
            else
            {
                _itemActionButton.onClick.AddListener(delegate { Engine.Instance.Hero.SetCurrentWeapon(_weapon); });
                _itemActionButton.gameObject.SetActive(true);
                //_itemActionButton.onClick.AddListener(delegate { Engine.Instance.RemoveItemFromInventory(slot._itemId, 1); });
                _itemActionButton.onClick.AddListener(delegate { UpdateActionButton(slot._quantity, Engine.Instance.GetItemByID(slot._itemId)); });
                _itemActionButtonText.text = _weapon.GetActionText();
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

    public void UpdateUnequipWeaponButton(Weapon weapon)
    {
        _unequipWeaponButton.onClick.RemoveAllListeners();
        _unequipWeaponButton.onClick.AddListener(delegate { ClearEquippedWeaponSlot(weapon); });
        _unequipWeaponButton.onClick.AddListener(delegate { _itemActionButton.gameObject.SetActive(true); });
        _unequipWeaponButton.onClick.AddListener(ClearCells);
        _unequipWeaponButton.onClick.AddListener(FillCells);
        _unequipWeaponButton.onClick.AddListener(SetCellsListeners);
    }

    public void UpdateEquippedWeaponImage(Sprite sprite)
    {
        _equippedWeaponImage.sprite = sprite;
    }

    public void ClearEquippedWeaponSlot(Weapon weapon)
    {
        if(weapon.Equipped)
        {
            Engine.Instance.Hero.SetCurrentWeapon(null);
            weapon.Unequip();
            UpdateEquippedWeaponImage(null);
        } else
        {
            Debug.Log("Weapon Unequipped");
        }
    }

    public void UpdateActionButton(int _quantity, Item item)
    {
        if (_quantity == 1 && item.GetType() != typeof(Weapon))
        {
            _itemActionButton.onClick.AddListener(HideInfoPanel);
        } else if (item.GetType() == typeof(Weapon))
        {
            Weapon _weapon = (Weapon)item;
            if(_weapon.Equipped)
            {
                _itemActionButton.gameObject.SetActive(false);
            }
        }
    }

}
