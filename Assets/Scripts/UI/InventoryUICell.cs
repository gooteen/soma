using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUICell : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private Text _itemQuantity;

    public void SetCellImage(Sprite image)
    {
        _itemImage.sprite = image;
    }

    public void SetCellQuantity(int quantity)
    {
        _itemQuantity.text = quantity.ToString();
    }

    public Button GetCellButton()
    {
        return gameObject.GetComponent<Button>();
    }
}
