using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoppedItem : MonoBehaviour
{
    [SerializeField] private float _delaySeconds;
    [SerializeField] private float _speed;
    private Rigidbody2D _rb;
    private SpriteRenderer _renderer;
    private Slot _itemSlot;
    private bool _isPulledToPlayer;

    void Awake()
    {
        _isPulledToPlayer = false;
        _rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (_isPulledToPlayer)
        {
            Debug.Log("pulling!");
            Debug.DrawLine(transform.position, Engine.Instance.Player.transform.position);
            //transform.Translate((Engine.Instance.Player.transform.position - transform.position).normalized * Time.deltaTime * _speed);
            _rb.AddForce((Engine.Instance.Player.transform.position - transform.position).normalized * _speed, ForceMode2D.Force);
        }
    }
    
    public void InitializePoppedItem(Sprite itemImage, Slot itemSlot)
    {
        _renderer.sprite = itemImage;
        _itemSlot = itemSlot;
        StartCoroutine("Delay");
    }

    public void Throw(Vector2 _dir, float _force)
    {
        _rb.AddForce(_dir.normalized * _force, ForceMode2D.Impulse);
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(_delaySeconds);
        _isPulledToPlayer = true;
        _rb.gravityScale = 0;
        //_rb.velocity = new Vector2(0,0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Engine.Instance.AddItemToInventory(_itemSlot._itemId, _itemSlot._quantity);
        Destroy(gameObject);
    }
}
