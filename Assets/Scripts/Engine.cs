using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    [SerializeField] private InputManager _input;
    //ссылка на класс-интерфейс игрока

    public InputManager InputManager { get { return _input; } }
    //геттер и сеттер на класс-интерфейс игрока
    public static Engine Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
