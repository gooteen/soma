using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    public int mode;

    //ссылка на класс-интерфейс игрока
    [SerializeField] private TacticalPlayerGateway _player;

    private InputManager _input;
    private TurnManager _turnManager;

    public InputManager InputManager { get { return _input; } }
    public TurnManager TurnManager { get { return _turnManager; } }
    
    //геттер и сеттер на класс-интерфейс игрока
    public TacticalPlayerGateway TacticalPlayer { get { return _player; } }

    public static Engine Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        _input = GetComponent<InputManager>();
        _turnManager = GetComponent<TurnManager>();
    }

    // TEMP __________________________________________________

    public void UpdatePlayerGateway(TacticalPlayerGateway player)
    {
        if (player == null)
        {
            _player = null;
            Debug.Log("Gateway null");
        }
        else
        {
            Debug.Log("Gateway: " + player);
        }
        _player = player;
    }

    public void ChangeMode()
    {
        mode++;

        if (mode == 3)
        {
            mode = 0;
        } 

        if (mode == 0)
        {
            CursorController.Instance.SetInRangeTiles();
            CursorController.Instance.ShowCursor();
        }
        else
        {
            CursorController.Instance.SetInRangeTiles();
            CursorController.Instance.HideCursor();
        }
    }
    // TEMP___________________________________________________

    public void InitializeTacticalPlayer(GameObject player)
    {
        _player = player.GetComponent<TacticalPlayerGateway>();
    }
}
