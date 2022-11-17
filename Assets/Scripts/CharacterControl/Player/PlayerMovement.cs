using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private CharacterAnimation _anim;
    [SerializeField] private float _movementOffset; 
    private Rigidbody2D _rb;
    [SerializeField] private bool _frozen;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate()
    {
        if (!_frozen)
        {
            Vector2 direction = Engine.Instance.InputManager.GetMovementDirection();
            Move(new Vector3(direction.x, direction.y));
            _anim.SetDirection(direction);
        }
    }

    public bool IsFrozen()
    {
        return _frozen;
    }

    public void Freeze()
    {
        _frozen = !_frozen;
        if (_frozen)
        {
            _rb.velocity = new Vector2(0, 0);
        }
    }

    public void Move(Vector2 direction)
    {
        //CharacterAnimation _anim = new CharacterAnimation();
        int param = _anim.DirectionToIndex(direction);

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
