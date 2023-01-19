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
    }

    private void OnEnable()
    {
        FillCells();
        SetCellsListeners();
        _cursorOccupied = false;
        DestroyHoveringImage();
        _cookingMethod = CookingMethod.Raw;
    }

    void Update()
    {
        if (_cursorOccupied)
        {
            HoverWithMouse();
        }
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
        if (Engine.Instance.InputManager.LeftMouseButtonPressed() && !HoveringOverCell())
        {
            Engine.Instance.AddItemToInventory(_lastClickedCell.GetItemID(), 1);
            _cursorOccupied = false;
            ClearCells();
            FillCells();
            SetCellsListeners();
            DestroyHoveringImage();
        }
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

    public void CreateHoveringImage()
    {
        _hoveringImageInstance = Instantiate(_hoveringImagePrefab, UIManager.Instance.gameObject.transform);
        _hoveringImageInstance.GetComponent<Image>().sprite = _lastClickedCell.GetCellImage().sprite;
    }

    public void DestroyHoveringImage()
    {
        if (_hoveringImageInstance != null)
        {
            Destroy(_hoveringImageInstance);
        }
        _hoveringImageInstance = null;
    }
}
