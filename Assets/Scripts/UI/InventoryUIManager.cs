using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryCellPrefab;
    [SerializeField] private List<InventoryUICell> _inventoryCellsList; 

    [SerializeField] private GameObject _itemInfoPanel;
    [SerializeField] private Transform _content;

    [SerializeField] private Button _itemActionButton;
    [SerializeField] private Text _itemActionButtonText;
    [SerializeField] private string _itemActionButtonTextValue;
    [SerializeField] private Text _itemName;
    [SerializeField] private Image _itemImage;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void FillCells()
    {
        List<Slot> _items = Engine.Instance.GetItemsInInvetory();
        foreach (Slot slot in _items)
        {
            GameObject _itemSlot = Instantiate(_inventoryCellPrefab, _content);
            InventoryUICell _cell = _itemSlot.GetComponent<InventoryUICell>();
            _inventoryCellsList.Add(_cell);
            Debug.Log("item id: " + slot._itemId);
            Item item = Engine.Instance.GetItemByID(slot._itemId);
            Debug.Log("Item: " + item);
            Debug.Log(" item image: " + (item.GetItemIcon()));
            _cell.SetCellImage(item.GetItemIcon());
            _cell.SetCellQuantity(slot._quantity);

            _cell.GetCellButton().onClick.AddListener(RemoveActionButtonListeners);
            _cell.GetCellButton().onClick.AddListener(ShowInfoPanel);
            _cell.GetCellButton().onClick.AddListener(delegate { FillInfoPanel(item, slot); } );
        }
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

    public void ClearCells() 
    {
        foreach (InventoryUICell cell in _inventoryCellsList)
        {
            Destroy(cell.gameObject);
        }
        _inventoryCellsList.Clear();
    }

    public void RemoveActionButtonListeners()
    {
        _itemActionButton.onClick.RemoveAllListeners();
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
            
        } else
        {
            Debug.LogError("Incorrect type");
        }
        _itemActionButton.onClick.AddListener(ClearCells);
        _itemActionButton.onClick.AddListener(FillCells);
    }

    public void UpdateActionButton(Slot slot)
    {
        if (slot._quantity == 1)
        {
            _itemActionButton.onClick.AddListener(HideInfoPanel);
        }
    }
}
