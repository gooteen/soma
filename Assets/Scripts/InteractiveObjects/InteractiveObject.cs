﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    [SerializeField] private float _distance;

    void Update()
    {
        if (IsFocusedOnObject())
        {
            if (Engine.Instance.InputManager.LeftMouseButtonPressed())
            {
                if ((Vector3.Distance(Engine.Instance.Player.transform.position, transform.position) <= _distance) && (!Engine.Instance.Player.IsFrozen()))
                {
                    Engine.Instance.Player.SetDirection(transform.position - Engine.Instance.Player.transform.position);
                    Debug.Log("Hit the interactive");
                    OnInteract();
                }
            }
        }
    }
    internal virtual void OnInteract()
    {

    }
    
    internal bool IsFocusedOnObject()
    {
        LayerMask _mask = LayerMask.GetMask("Interactive");

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
