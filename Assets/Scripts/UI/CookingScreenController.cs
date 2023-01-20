using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//на клик при перетаскивании айтема в любом случае сбрасываем айтем, ЕСЛИ курсор не направлен на кукнг селл
public class CookingScreenController : InventoryUIManager
{
    [SerializeField] private CraftingUICell[] _cookingCells;
    [SerializeField] private InventoryUICell _resultCell;

    [SerializeField] private Dropdown _dropdown;

    [SerializeField] private CookingMethod _cookingMethod;

    [SerializeField] private CookingBook _cookingBook;

    [SerializeField] private GameObject _hoveringImagePrefab;
    [SerializeField] private GameObject _hoveringImageInstance;

    [SerializeField] private InventoryUICell _lastClickedCell;

    [SerializeField] private List<Slot> _ingredientsReady;
    [SerializeField] private Slot _resultSlot;
    [SerializeField] private bool _cursorOccupied;

    void Start()
    {
        
    }

    //temporary
    private void OnDisable()
    {
        ClearCells();
        _cursorOccupied = false;
        DestroyHoveringImage();
        _cookingMethod = CookingMethod.Raw;
        _dropdown.value = 0;
        SetCookingMethod();
        ClearResultCell();
        ReturnItemsToInventory();
        ClearCraftingCells();
    }

    private void OnEnable()
    {
        FillCells();
        ClearCraftingCells();
        SetCellsListeners();
        _cursorOccupied = false;
        DestroyHoveringImage();

        _dropdown.value = 0;
        SetCookingMethod();
    }

    void Update()
    {
        if (_cursorOccupied)
        {
            HoverWithMouse();
        }
        ProcessMouseClick();
    }

    public void SetCookingMethod()
    {
        switch(_dropdown.value)
        {
            case 0:
                _cookingMethod = CookingMethod.Raw;
                break;
            case 1:
                _cookingMethod = CookingMethod.Fryed;
                break;
            case 2:
                _cookingMethod = CookingMethod.Baked;
                break;
        }
        GenerateResult();
    }

    public override void SetCellsListeners()
    {
        foreach (InventoryUICell cell in _inventoryCellsList)
        {
            cell.GetCellButton().onClick.AddListener(delegate { Engine.Instance.RemoveItemFromInventory(cell.GetItemID(), 1); });
            cell.GetCellButton().onClick.AddListener(delegate { SetCurrentCell(cell); });
            cell.GetCellButton().onClick.AddListener(delegate { Debug.Log("I'm working!"); });
            cell.GetCellButton().onClick.AddListener(CreateHoveringImage);
        }
    }

    public void SetCurrentCell(InventoryUICell cell)
    {
        _lastClickedCell = cell;
        _cursorOccupied = true;
        ClearCells();
        FillCells();
    }

    public void HoverWithMouse()
    {
        Vector2 _mousePos = Engine.Instance.InputManager.GetMousePosition();
        _hoveringImageInstance.transform.position = _mousePos;
    }

    public void ProcessMouseClick()
    {
        if (Engine.Instance.InputManager.LeftMouseButtonPressed() && !HoveringOverCell())
        {
            if(_cursorOccupied)
            {
                Debug.Log("ERRORSHMERROR: ...");
                ReturnItemOnMouseToInventory();
            }
        }
        else if (Engine.Instance.InputManager.LeftMouseButtonPressed() && HoveringOverCell())
        {
            Debug.Log("ERRORSHMERROR: ..2.");

            int _cellIndex = GetSelectedCellIndex();
            CraftingUICell _cell = _cookingCells[_cellIndex];
            if(!_cell._isOccupied)
            {
                if (_cursorOccupied)
                {
                    Debug.Log("ERRORSHMERROR: cursor is occupied");
                    SetCraftingCell(_cellIndex);
                    ClearCells();
                    FillCells();
                    SetCellsListeners();
                    DestroyHoveringImage();
                    GenerateResult();
                    _cell.SetOccupied();
                }
            } else
            {
                Debug.Log("Conditions: cursor: " + _cursorOccupied + ", slot: " + _cell._isOccupied); 
                if(_cursorOccupied)
                {
                    Debug.Log("SHMERROR CHECK: cell highlighted: " + _cell.GetItemID() + " lact clicked cell: " + _lastClickedCell.GetItemID());
                    if (_cell.GetItemID() != _lastClickedCell.GetItemID())
                    {
                        Debug.Log("ERRORSHMERROR: cell is occupied");
                        ReturnItemOnMouseToInventory();
                    }
                    else
                    {
                        Debug.Log("ERRORSHMERROR: supposed to add one item to the slot");
                        UpdateCraftingCell(_cellIndex);
                        ClearCells();
                        FillCells();
                        SetCellsListeners();
                        DestroyHoveringImage();
                        GenerateResult();
                    }
                } else
                {
                    Debug.Log("ERRORSHMERROR: supposed to delete an item from the crafting cells");
                    Engine.Instance.AddItemToInventory(_cell.GetItemID(), _ingredientsReady[_cellIndex]._quantity);
                    _ingredientsReady[_cellIndex] = new Slot(ItemID.Null, 0);
                    ClearCraftingCell(_cellIndex);
                    ClearCells();
                    FillCells();
                    SetCellsListeners();
                    GenerateResult();
                }

            }
        }
    }

    public void ReturnItemOnMouseToInventory()
    {
        Engine.Instance.AddItemToInventory(_lastClickedCell.GetItemID(), 1);
        ClearCells();
        FillCells();
        SetCellsListeners();
        DestroyHoveringImage();
    }

    public void UpdateCraftingCell(int cellIndex)
    {
        CraftingUICell _cell = _cookingCells[cellIndex];
        _ingredientsReady[cellIndex].IncreaseQuantity(1);
        _cell.SetCellQuantity(_ingredientsReady[cellIndex]._quantity);
    }

    public void SetCraftingCell(int cellIndex)
    {
        CraftingUICell _cell = _cookingCells[cellIndex];
        _ingredientsReady[cellIndex] = new Slot(_lastClickedCell.GetItemID(), 1);
        _cell.SetItemID(_ingredientsReady[cellIndex]._itemId);
        _cell.SetCellImage(Engine.Instance.GetItemByID(_ingredientsReady[cellIndex]._itemId).GetItemIcon());
        _cell.ShowCellQuantity();
        _cell.SetCellQuantity(_ingredientsReady[cellIndex]._quantity);
    }

    public void ClearCraftingCell(int cellIndex)
    {
        _ingredientsReady[cellIndex] = new Slot(ItemID.Null, 0);
        CraftingUICell _cell = _cookingCells[cellIndex];
        _cell.SetCellImage(null);
        _cell.HideCellQuantity();
        _cell.SetUnoccupied();
    }

    public bool HoveringOverCell()
    {
        foreach (CraftingUICell cell in _cookingCells)
        {
            if (cell._isHoveredOver == true)
            {
                return true;
            }
        }
        return false;
    }

    public void ReturnItemsToInventory()
    {
        foreach (Slot ingredient in _ingredientsReady)
        {
            if (ingredient._itemId != ItemID.Null)
            {
                Engine.Instance.AddItemToInventory(ingredient._itemId, ingredient._quantity);
            }
        }
    }

    public void GenerateResult()
    {
        if (_resultSlot._itemId != ItemID.Null)
        {
            _resultSlot = new Slot(ItemID.Null, 0);
            ClearResultCell();
        }

        foreach (Recipe recipe in _cookingBook._recipes)
        {
            Debug.Log("Count? " + _ingredientsReady.Count);

            Slot _result = recipe.TryCooking(_ingredientsReady, _cookingMethod);
            if (_result._itemId != ItemID.Null)
            {
                _resultSlot = _result;
                SetResultCell();
            }
        }
    }

    public void ClearResultCell()
    {
        _resultCell.SetCellImage(null);
        _resultCell.HideCellQuantity();
        _resultCell.GetCellButton().onClick.RemoveAllListeners();
    }

    public void SetResultCell()
    {
        _resultCell.SetCellImage(Engine.Instance.GetItemByID(_resultSlot._itemId).GetItemIcon());
        _resultCell.SetCellQuantity(_resultSlot._quantity);
        _resultCell.ShowCellQuantity();
        _resultCell.GetCellButton().onClick.RemoveAllListeners();
        _resultCell.GetCellButton().onClick.AddListener(delegate { Engine.Instance.AddItemToInventory(_resultSlot._itemId, _resultSlot._quantity); });
        _resultCell.GetCellButton().onClick.AddListener(ClearResultCell);
        _resultCell.GetCellButton().onClick.AddListener(delegate { AddExcessItemsToInvetory(_resultSlot._quantity); });
        _resultCell.GetCellButton().onClick.AddListener(ClearCraftingCells);
        _resultCell.GetCellButton().onClick.AddListener(ClearCells);
        _resultCell.GetCellButton().onClick.AddListener(FillCells);
        _resultCell.GetCellButton().onClick.AddListener(SetCellsListeners);
    }

    public void AddExcessItemsToInvetory(int quantity)
    {
        for (int i = 0; i < _ingredientsReady.Count; i++)
        {
            if (_ingredientsReady[i]._quantity > quantity)
            {
                int numItemsToBeAdded = _ingredientsReady[i]._quantity - quantity;
                Engine.Instance.AddItemToInventory(_ingredientsReady[i]._itemId, numItemsToBeAdded);
            }
        }
    }

    public void ClearCraftingCells()
    {
        for (int i = 0; i < _cookingCells.Length; i++)
        {
            ClearCraftingCell(i);
        }
    }

    public int GetSelectedCellIndex()

    {
        for (int i = 0; i < _cookingCells.Length; i++)
        {
            if (_cookingCells[i]._isHoveredOver)
            {
                return i;
            }
        }
        return 0;
    }

    public void CreateHoveringImage()
    {
        _hoveringImageInstance = Instantiate(_hoveringImagePrefab, UIManager.Instance.gameObject.transform);
        _hoveringImageInstance.GetComponent<Image>().sprite = _lastClickedCell.GetCellImage().sprite;
    }

    public void DestroyHoveringImage()
    {
        _cursorOccupied = false;
        if (_hoveringImageInstance != null)
        {
            Destroy(_hoveringImageInstance);
        }
        _hoveringImageInstance = null;
    }
}
