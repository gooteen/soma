using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUICell : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private Text _itemQuantity;
    [SerializeField] private GameObject _quantityBox;
    [SerializeField] private ItemID _itemID;

    public void SetCellImage(Sprite image)
    {
        _itemImage.sprite = image;
    } 
    public Image GetCellImage()
    {
        return _itemImage;
    }

    public void SetCellQuantity(int quantity)
    {
        _itemQuantity.text = quantity.ToString();
    }

    public void HideCellQuantity()
    {
        _quantityBox.SetActive(false);
    }

    public void ShowCellQuantity()
    {
        _quantityBox.SetActive(true);
    }

    public Button GetCellButton()
    {
        return gameObject.GetComponent<Button>();
    }

    public void SetItemID(ItemID id)
    {
        _itemID = id;
    }

    public ItemID GetItemID()
    {
        return _itemID;
    }
}
