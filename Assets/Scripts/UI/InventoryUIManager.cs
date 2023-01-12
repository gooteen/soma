using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryCellPrefab;
    [SerializeField] private List<GameObject> _inventoryCellsList; //remake into a list of UIInventaryCell instances

    [SerializeField] private Button _itemActionButton;
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

    }

    public void ClearCells() 
    {

    }
}
