using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private PlayerAnimation anim;
    private Rigidbody2D _rb;

    void Start()
    {
        Debug.Log(Mathf.FloorToInt(0.8f));
        _rb = GetComponent<Rigidbody2D>();
        //input.PlayerMovement.Run.performed += Move;
    }
    
    void FixedUpdate()
    {
        Vector2 direction = InputManager.Instance.GetMovementDirection();
        Move(direction);
        anim.SetDirection(direction);
    }

    public void Move(Vector2 direction)
    {
        _rb.velocity = direction.normalized * _movementSpeed;
    }
}
