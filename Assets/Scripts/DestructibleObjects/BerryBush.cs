using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryBush : InteractiveObject
{
    [SerializeField] Sprite _full;
    [SerializeField] Sprite _empty;
    private bool _isFull = true;
    private SpriteRenderer _renderer;

    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.sprite = _full;
    }

    internal override void OnInteract()
    {
        Debug.Log("Here");
        if (_isFull)
        {
            _isFull = false;
            _renderer.sprite = _empty;
        }
    }
}
