using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private PlayerAnimation anim;
    private Rigidbody2D _rb;
    private PlayerInputActions _input;

    void Start()
    {
        Debug.Log(Mathf.FloorToInt(0.8f));
        _input = new PlayerInputActions();
        _input.PlayerMovement.Enable();
        _rb = GetComponent<Rigidbody2D>();
        //input.PlayerMovement.Run.performed += Move;
    }
    
    void FixedUpdate()
    {
        Vector2 direction = _input.PlayerMovement.Run.ReadValue<Vector2>();
        Move(direction);
        anim.SetDirection(direction);
    }

    public void Move(Vector2 direction)
    {
        _rb.velocity = direction.normalized * _movementSpeed;
    }
}
