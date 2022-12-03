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
    }
    internal override void OnInteract()
    {
        if (_interactive)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D col) 
    {
        if (col.gameObject.tag == "Player")
        {
            if (Engine.Instance.Player.CanPickUpItems())
            {
                Destroy(this.gameObject);
                Debug.Log("worked");
            } else
            {
                Debug.Log("1хуй");
            }
        } else
        {
            Debug.Log("2хуй");
        }
    }

    public void Drop()
    {
        StartCoroutine("FallCoroutine");
    }

    private IEnumerator FallCoroutine()
    {
        _rb2d.isKinematic = false;
       yield return new WaitForSeconds(0.1f);
        _sprite.sortingOrder = _sortOrdStart;
        yield return new WaitForSeconds(0.7f);
        _sprite.sortingOrder = _sortOrdEnd;
        _interactive = true;
    }
}
