using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    private PlayerInputActions _input;

    void Start()
    {
        _input = new PlayerInputActions();
        _input.PlayerMovement.Enable();
        _input.Mouse.Enable();
    }

    public Vector2 GetMovementDirection()
    {
        return _input.PlayerMovement.Run.ReadValue<Vector2>();
    }

    public Vector2 GetMousePosition()
    {
        return _input.Mouse.MousePosition.ReadValue<Vector2>();
    }

    public bool LeftMouseButtonPressed()
    {
        return _input.Mouse.LeftButtonPressed.triggered;
    }
}
