using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSprinkler : MonoBehaviour
{
    [SerializeField] private GameObject _poppedItemPrefab;
    [SerializeField] private Transform _spawningPoint;

    [SerializeField] private float _pauseBetweenThrows = 0.25f;
    [SerializeField] private float _forceToSprinkleWith = 7;

    [SerializeField] private bool _sprinkleEachItemOfType;

    private Slot[] _itemsToSprinkle;
    private bool _throwToTheRight;

    void Start()
    {
        _itemsToSprinkle = null;
        _throwToTheRight = false;
        //TEMP
        //StartSpawningItems(new Slot[] { new Slot(ItemID.Wheat, 1), new Slot(ItemID.Apple, 1) });
    }

    public void StartSpawningItems(Slot[] items)
    {
        _itemsToSprinkle = items;
        StartCoroutine("Sprinkle");
    }

    private IEnumerator Sprinkle()
    {
        if(_sprinkleEachItemOfType)
        {
            for (int i = 0; i < _itemsToSprinkle.Length; i++)
            {
                for (int j = 1; j <= _itemsToSprinkle[i]._quantity; j++)
                {
                    SprinkleItem(i);
                    yield return new WaitForSeconds(_pauseBetweenThrows);
                }
            }
        } else
        {
            for (int i = 0; i < _itemsToSprinkle.Length; i++)
            {
                SprinkleItem(i);
                yield return new WaitForSeconds(_pauseBetweenThrows);
            }
        }
        
    }

    private void SprinkleItem(int i)
    {
        GameObject _newPoppedItemObject = Instantiate(_poppedItemPrefab);

        _newPoppedItemObject.transform.position = new Vector3(_spawningPoint.position.x, _spawningPoint.position.y, _newPoppedItemObject.transform.position.z);

        PoppedItem _newPoppedItem = _newPoppedItemObject.GetComponent<PoppedItem>();

        Debug.Log("Sprinkler index: " + i);
        Item _item = Engine.Instance.GetItemByID(_itemsToSprinkle[i]._itemId);
        _newPoppedItem.InitializePoppedItem(_item.GetItemIcon(), _itemsToSprinkle[i], _sprinkleEachItemOfType);
        Vector2 _throwDirection;

        if (!_throwToTheRight)
        {
            _throwDirection = new Vector2(-0.5f, 1);
        } else
        {
            _throwDirection = new Vector2(0.5f, 1);
        }

        _newPoppedItem.Throw(_throwDirection, _forceToSprinkleWith);
        _throwToTheRight = !_throwToTheRight;
    }
}
