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
        CheckIfRightWeaponIsEquipped();
    }

    /*
    private void Start() 
    {
        Eatable _eatable = (Eatable)_inventory.FindItemInMap(ItemID.Wheat)._item;

        if (Instance._currentGameMode == GameMode.Battle)
        {
            //
        }
        else
        {
            Instance.Player.RestoreHealth(_eatable.GetHealthToRestore());
        }
        RemoveItemFromInventory(ItemID.Wheat, 1);
    }
    */
    
    public Item GetItemByID(ItemID id)
    {
        return _inventory.FindItemInMap(id)._item;
    }

    public Slot GetItemSlotByID(ItemID id)
    {
        return _inventory.FindSlotInInventory(id);
    }

    public void AddItemToInventory(ItemID id, int quantity)
    {
        _inventory.AddItem(id, quantity);
    }

    public void RemoveItemFromInventory(ItemID id, int quantity)
    {
        _inventory.RemoveItem(id, quantity);
    }

    public List<Slot> GetItemsInInvetory()
    {
        return _inventory._items;
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

    private void CheckIfRightWeaponIsEquipped()
    {
        Weapon _weaponToSet = null;
        foreach (Slot item in _inventory._items)
        {
            Item _item = GetItemByID(item._itemId);
            if (_item.GetType() == typeof(Weapon))
            {
                Weapon _weapon = (Weapon)_item;
                if (_weapon.Equipped && Hero.CurrentWeapon != _weapon)
                {
                    Hero.SetCurrentWeapon(_weapon);
                    _weaponToSet = _weapon;
                }
            }
        }
        if (_weaponToSet == null)
        {
            Hero.SetCurrentWeapon(null);
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
