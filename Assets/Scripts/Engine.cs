using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    private InputManager _input;

    //ссылка на класс-интерфейс игрока
    [SerializeField] private TacticalPlayerGateway _player;

    public InputManager InputManager { get { return _input; } }
    
    //геттер и сеттер на класс-интерфейс игрока
    public TacticalPlayerGateway TacticalPlayer { get { return _player; } }

    public static Engine Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        _input = GetComponent<InputManager>();
    }

    public void InitializeTacticalPlayer(GameObject player)
    {
        _player = player.GetComponent<TacticalPlayerGateway>();
    }
}
