using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TacticalMode { Movement, Combat };
public enum GameMode { Exploration, Battle };
public class Engine : MonoBehaviour
{
    public GameMode _currentGameMode;
    public TacticalMode _currentTacticalMode;

    [SerializeField] private Inventory _inventory;

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

    public void AddItemToInventory(ItemID id, int quantity)
    {
        _inventory.AddItem(id, quantity);
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
        _currentTacticalMode = mode;
        if (mode == TacticalMode.Movement)
        {
            CursorController.Instance.ShowCursor();
        }
        else
        {
            CursorController.Instance.HideCursor();
        }
    }

    public void ChangeGameMode()
    {
        if (_currentGameMode == GameMode.Battle)
        {
            _currentGameMode = GameMode.Exploration;
        } else
        {
            _currentGameMode = GameMode.Battle;
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
