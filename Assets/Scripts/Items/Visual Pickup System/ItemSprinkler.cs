using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSprinkler : MonoBehaviour
{
    [SerializeField] private GameObject _poppedItemPrefab;
    [SerializeField] private Transform _spawningPoint;

    [SerializeField] private float _pauseBetweenThrows;
    [SerializeField] private float _forceToSprinkleWith;

    private Slot[] _itemsToSprinkle;

    void Start()
    {
        _itemsToSprinkle = null;

        //TEMP
        StartSpawningItems(new Slot[] { new Slot(ItemID.Wheat, 1), new Slot(ItemID.Apple, 1) });
    }

    void Update()
    {
        
    }

    public void StartSpawningItems(Slot[] items)
    {
        _itemsToSprinkle = items;
        StartCoroutine("Sprinkle");
    }

    private IEnumerator Sprinkle()
    {
        for (int i = 0; i < _itemsToSprinkle.Length; i++)
        {
            GameObject _newPoppedItemObject = Instantiate(_poppedItemPrefab);

            _newPoppedItemObject.transform.position = new Vector3(_spawningPoint.position.x, _spawningPoint.position.y, _newPoppedItemObject.transform.position.z);

            PoppedItem _newPoppedItem = _newPoppedItemObject.GetComponent<PoppedItem>();

            Item _item = Engine.Instance.GetItemByID(_itemsToSprinkle[i]._itemId);
            _newPoppedItem.InitializePoppedItem(_item.GetItemIcon(), _itemsToSprinkle[i]);
            Vector2 _throwDirection;

            if (i % 2 == 0)
            {
                _throwDirection = new Vector2(-0.5f, 1);
            } else
            {
                _throwDirection = new Vector2(0.5f, 1);
            }

            _newPoppedItem.Throw(_throwDirection, _forceToSprinkleWith);

            yield return new WaitForSeconds(_pauseBetweenThrows);
        }
    }
}
