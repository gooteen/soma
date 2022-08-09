using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    private GameObject _focusedTile;

    void Start()
    {
        
    }

    void LateUpdate()
    {
        Debug.Log(GetFocusedOnTile());
        
        if (GetFocusedOnTile())
        {
            transform.position = _focusedTile.transform.position;
            transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder = _focusedTile.GetComponent<SpriteRenderer>().sortingOrder;
            if(InputManager.Instance.LeftMouseButtonPressed())
            {
                _focusedTile.GetComponent<OverlayTile>().ShowOverlay();
            }
        }
    }

    
    public bool GetFocusedOnTile()
    {
        // The camera component tagged "MainCamera"
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(InputManager.Instance.GetMousePosition());
        Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2d, Vector2.zero, Mathf.Infinity, _layerMask, -Mathf.Infinity, Mathf.Infinity);
        if (hit.collider != null)
        {
            _focusedTile = hit.collider.gameObject;
            Debug.Log("Hit " + hit.collider.gameObject);
            Debug.DrawRay(mousePos2d, Vector2.zero * 10);
            return true;
        } else
        {
            return false;
        }
    }
}
