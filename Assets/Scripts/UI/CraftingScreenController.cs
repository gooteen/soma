using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingScreenController : InventoryUIManager
{
    [SerializeField] private GameObject _blueprintResourceCellPrefab;
    [SerializeField] private Button _actionButton;

    [SerializeField] private Text _blueprintItemName;
    [SerializeField] private Image _blueprintItemImage;

    [SerializeField] private Transform _blueprintCellsContent;
    [SerializeField] private Transform _blueprintResourceCellsContent;

    [SerializeField] private List<InventoryUICell> _blueprintCells;
    [SerializeField] private List<InventoryUICell> _blueprintResourceCells;

    [SerializeField] private BlueprintList _blueprintList;

    private void OnEnable()
    {
        FillCells();
        HideInfoPanel();
        FillBlueprintCells();
    }

    private void OnDisable()
    {
        ClearCells();
        ClearBlueprintCells();
        ClearInfoPanel();
    }

    public override void FillCells()
    {
        List<Slot> _items = Engine.Instance.GetItemsInInvetory();
        foreach (Slot slot in _items)
        {
            if (Engine.Instance.GetItemByID(slot._itemId).GetType() == typeof(Resource))
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
    }

    public void HideActionButton()
    {
        _actionButton.gameObject.SetActive(false);

    }

    public void ShowActionButton()
    {
        _actionButton.gameObject.SetActive(true);
    }

    public void FillBlueprintCells()
    {
        foreach (Blueprint blueprint in _blueprintList._blueprints)
        {
            GameObject _itemSlot = Instantiate(_inventoryCellPrefab, _blueprintCellsContent);
            InventoryUICell _cell = _itemSlot.GetComponent<InventoryUICell>();
            _blueprintCells.Add(_cell);
            Item _item = Engine.Instance.GetItemByID(blueprint._itemToBeCrafted);

            _cell.SetItemID(blueprint._itemToBeCrafted);
            _cell.SetCellImage(_item.GetItemIcon());
            _cell.HideCellQuantity();
            _cell.GetCellButton().onClick.AddListener(delegate { FillInfoPanel(blueprint); });
        }
    }

    public void FillInfoPanel(Blueprint blueprint)
    {
        ShowInfoPanel(); 
        ClearInfoPanel();
        Item _item = Engine.Instance.GetItemByID(blueprint._itemToBeCrafted);
        _blueprintItemName.text = _item.GetItemName();
        _blueprintItemImage.sprite = _item.GetItemIcon();
        FillBlueprintResourceCells(blueprint);
        SetActionButton(blueprint);
    }

    public void ClearInfoPanel()
    {
        _blueprintItemName.text = null;
        _blueprintItemImage.sprite = null;
        ClearBlueprintResourceCells();
    }

    public void SetActionButton(Blueprint blueprint)
    {
        HideActionButton();
        _actionButton.onClick.RemoveAllListeners();
        if (blueprint.ItemCanBeCrafted())
        {
            ShowActionButton();
            _actionButton.onClick.AddListener(blueprint.Craft);
            _actionButton.onClick.AddListener(ClearCells);
            _actionButton.onClick.AddListener(FillCells);
            _actionButton.onClick.AddListener(delegate { SetActionButton(blueprint); } );
        }
    }

    public void FillBlueprintResourceCells(Blueprint blueprint)
    {
        foreach (Slot resource in blueprint._resourcesNeeded)
        {
            GameObject _itemSlot = Instantiate(_blueprintResourceCellPrefab, _blueprintResourceCellsContent);
            InventoryUICell _cell = _itemSlot.GetComponent<InventoryUICell>();
            _blueprintResourceCells.Add(_cell);

            Item _item = Engine.Instance.GetItemByID(resource._itemId);

            _cell.SetItemID(resource._itemId);
            _cell.SetCellImage(_item.GetItemIcon());
            _cell.SetCellQuantity(resource._quantity);
        }
    }

    public void ClearBlueprintResourceCells()
    {
        foreach (InventoryUICell cell in _blueprintResourceCells)
        {
            Destroy(cell.gameObject);
        }
        _blueprintResourceCells.Clear();
    }

    public void ClearBlueprintCells()
    {
        foreach (InventoryUICell cell in _blueprintCells)
        {
            Destroy(cell.gameObject);
        }
        _blueprintCells.Clear();
    }

    public override void SetCellsListeners()
    {
        throw new System.NotImplementedException();
    }

    public void HideInfoPanel()
    {
        _actionButton.gameObject.SetActive(false);
        _blueprintResourceCellPrefab.SetActive(false);
        _blueprintItemName.gameObject.SetActive(false);
        _blueprintItemImage.gameObject.SetActive(false);
    }

    public void ShowInfoPanel()
    {
        _actionButton.gameObject.SetActive(true);
        _blueprintResourceCellPrefab.SetActive(true);
        _blueprintItemName.gameObject.SetActive(true);
        _blueprintItemImage.gameObject.SetActive(true);
    }
}
