using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //переделать из синглтона обратно, обращаться через Engine
    private static InputManager _instance;
    private PlayerInputActions _input;
    public static InputManager Instance { get { return _instance; } }

    void Awake()
    {
        if (_instance != null & !_instance != this)
        {
            Destroy(this.gameObject);
        } else
        {
            _instance = this;
        }
    }

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
