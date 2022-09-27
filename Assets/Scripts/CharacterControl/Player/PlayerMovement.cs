using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private CharacterAnimation anim;
    [SerializeField] private float _movementOffset; 
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate()
    {
        Vector2 direction = Engine.Instance.InputManager.GetMovementDirection();
        Move(new Vector3(direction.x, direction.y));
        anim.SetDirection(direction);
    }

    public void Move(Vector2 direction)
    {
        CharacterAnimation _anim = new CharacterAnimation();
        int param = anim.DirectionToIndex(direction);

        //directing the movement along the isometric axes 

        if (param == 1)
        {
            direction = new Vector2(direction.x - _movementOffset, direction.y);
        } else if (param == 5)
        {
            direction = new Vector2(direction.x + _movementOffset, direction.y);
        } else if (param == 7)
        {
            direction = new Vector2(direction.x + _movementOffset, direction.y);
        } else if (param == 3)
        {
            direction = new Vector2(direction.x - _movementOffset, direction.y);
        } 

        Debug.Log("param: " + param);
        _rb.velocity = direction.normalized * _movementSpeed;
    }
}
