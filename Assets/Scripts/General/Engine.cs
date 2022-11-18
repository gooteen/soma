using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TacticalMode { Movement, Combat };
public class Engine : MonoBehaviour
{
    public int mode;
    public TacticalMode _currentMode;

    //ссылка на класс-интерфейс текущего игрока в тактическом режиме
    [SerializeField] private TacticalPlayerGateway _tacticalPlayer;
    [SerializeField] private PlayerGateway _player;
    [SerializeField] private HeroStats _mainHeroStats;

    private InputManager _input;
    private TurnManager _turnManager;

    public HeroStats Hero { get { return _mainHeroStats; } }
    public InputManager InputManager { get { return _input; } }
    public TurnManager TurnManager { get { return _turnManager; } }
    
    //геттер и сеттер на класс-интерфейс игрока
    public TacticalPlayerGateway TacticalPlayer { get { return _tacticalPlayer; } }
    public PlayerGateway Player { get { return _player; } }

    public static Engine Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        _input = GetComponent<InputManager>();
        _turnManager = GetComponent<TurnManager>();
    }

    // TEMP __________________________________________________

    public void HidePlayer()
    {
        _player.HidePlayer();
    }

    public void ShowPLayer()
    {
        _player.ShowPlayer();
    }

    public void UpdatePlayerGateway(TacticalPlayerGateway player)
    {
        if (player == null)
        {
            _tacticalPlayer = null;
            Debug.Log("Gateway null");
        }
        else
        {
            Debug.Log("Gateway: " + player);
        }
        _tacticalPlayer = player;
    }

    public void ChangeTacticalMode(TacticalMode mode)
    {
        _currentMode = mode;
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
    /*
    public void InitializeTacticalPlayer(GameObject player)
    {
        _tacticalPlayer = player.GetComponent<TacticalPlayerGateway>();
    }
    */
}
