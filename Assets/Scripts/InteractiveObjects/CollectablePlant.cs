using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablePlant : InteractiveObject
{
    [SerializeField] Sprite _full;
    [SerializeField] Sprite _empty;
    private bool _isFull = true;
    private SpriteRenderer _renderer;

    internal override void Start()
    {
        _interactive = true;
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.sprite = _full;
    }

    internal override void OnInteract()
    {
        Debug.Log("Here");
        if (_interactive)
        {
            _isFull = false;
            _renderer.sprite = _empty;
            _interactive = false;
        }
    }
}
