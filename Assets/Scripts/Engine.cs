using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    public int mode;

    //ссылка на класс-интерфейс игрока
    [SerializeField] private TacticalPlayerGateway _player;

    private InputManager _input;

    public InputManager InputManager { get { return _input; } }
    
    //геттер и сеттер на класс-интерфейс игрока
    public TacticalPlayerGateway TacticalPlayer { get { return _player; } }

    public static Engine Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        _input = GetComponent<InputManager>();
    }

    // TEMP __________________________________________________

    public void ChangeMode()
    {
        if (mode == 1)
        {
            mode = 0;
            CursorController.Instance.SetInRangeTiles();
            CursorController.Instance.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            mode = 1;
            CursorController.Instance.SetInRangeTiles();
            CursorController.Instance.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    // TEMP___________________________________________________


    public void InitializeTacticalPlayer(GameObject player)
    {
        _player = player.GetComponent<TacticalPlayerGateway>();
    }
}
