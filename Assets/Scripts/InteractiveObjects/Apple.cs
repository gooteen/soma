using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : InteractiveObject
{
    [SerializeField] private int _sortOrdStart;
    [SerializeField] private int _sortOrdEnd;

    private SpriteRenderer _sprite;
    private Rigidbody2D _rb2d;

    internal override void Start()
    {
        base.Start();
        _sprite = GetComponent<SpriteRenderer>();
        _rb2d = GetComponent<Rigidbody2D>();
        _sprite.sortingOrder = _sortOrdStart;
    }
    internal override void OnInteract()
    {
        if (_interactive)
        {
            Destroy(gameObject);
        }
    }

    public void Drop()
    {
        StartCoroutine("FallCoroutine");
    }

    private IEnumerator FallCoroutine()
    {
        _rb2d.isKinematic = false;
        yield return new WaitForSeconds(0.4f);
        _sprite.sortingOrder = _sortOrdEnd;
        yield return new WaitForSeconds(0.33f);
        _sprite.sortingOrder = _sortOrdStart;
        _interactive = true;
    }
}
