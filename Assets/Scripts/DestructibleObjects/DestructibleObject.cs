using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [SerializeField] private float _distance;
    [SerializeField] private int _hitsToDestroy;

    void Update()
    {
        if (IsFocusedOnDestructibleObject())
        {
            if (Engine.Instance.InputManager.LeftMouseButtonPressed())
            {
                if ((Vector3.Distance(Engine.Instance.Player.transform.position, transform.position) <= _distance) && (!Engine.Instance.Player.IsFrozen()))
                {
                    Engine.Instance.Player.SetDirection(transform.position - Engine.Instance.Player.transform.position);
                    Debug.Log("Hit the destructible");
                    _hitsToDestroy--;
                    if(_hitsToDestroy <= 0) 
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        Debug.Log("Dropped some shit");
    }

    public bool IsFocusedOnDestructibleObject()
    {
        LayerMask _mask = LayerMask.GetMask("Destructible");

        Vector3 _mousePos = Camera.main.ScreenToWorldPoint(Engine.Instance.InputManager.GetMousePosition());
        Vector2 _mousePos2d = new Vector2(_mousePos.x, _mousePos.y);

        RaycastHit2D _hit = Physics2D.Raycast(_mousePos2d, Vector2.zero, Mathf.Infinity, _mask, -Mathf.Infinity, Mathf.Infinity);

        if (_hit.collider != null && Engine.Instance.TacticalPlayer == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
