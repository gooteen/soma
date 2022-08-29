﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private CharacterAnimation anim;
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate()
    {
        Vector2 direction = Engine.Instance.InputManager.GetMovementDirection();
        Move(direction);
        anim.SetDirection(direction);
    }

    public void Move(Vector2 direction)
    {
        _rb.velocity = direction.normalized * _movementSpeed;
    }
}
