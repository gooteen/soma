using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
// TEMP
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public OverlayTile overlayTilePrefab;
    public GameObject overlayContainer;

    public Dictionary<Vector2Int, OverlayTile> map;

    public static MapManager Instance { get { return _instance; } }
    private static MapManager _instance;

    [SerializeField] CombatInitiator _initiator;
    [SerializeField] Text PlayerTextHP;
    [SerializeField] Text PlayerTextAP;
    [SerializeField] Text EnemyTextHP;
    [SerializeField] Text EnemyTextAP;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else
        {
            _instance = this;
        }
    }

    void Start()
    {
        //InitializeArena();
    }

    // Update is called once per frame
    void Update()
    {
        // TEMP
        /*
        if (GameObject.Find("EnemyTactical(Clone)") != null)
        {
            EnemyTextHP.text = $"Enemy HP: {GameObject.Find("EnemyTactical(Clone)").GetComponent<TacticalCharacterInfo>().GetHealthPoints().ToString()}";
        }
        if (GameObject.Find("PlayerTactical(Clone)") != null)
        {
            PlayerTextHP.text = $"Player HP: {GameObject.Find("PlayerTactical(Clone)").GetComponent<TacticalCharacterInfo>().GetHealthPoints().ToString()}";
        }
        if (GameObject.Find("EnemyTactical(Clone)") != null)
        {
            EnemyTextAP.text = $"Enemy AP: {GameObject.Find("EnemyTactical(Clone)").GetComponent<TacticalCharacterInfo>().GetActionPoints().ToString()}";
        }
        if (GameObject.Find("PlayerTactical(Clone)") != null)
        {
            PlayerTextAP.text = $"Player AP: {GameObject.Find("PlayerTactical(Clone)").GetComponent<TacticalCharacterInfo>().GetActionPoints().ToString()}";
        }
        */
        //PrintMap();
    }

    void PrintMap()
    {
        foreach (var pair in map)
        {
            Debug.Log($"key: {pair.Key} value : {pair.Value}");
        }
    }

    public void SetInitiator(GameObject initiator)
    {
        _initiator = initiator.GetComponent<CombatInitiator>();
        Debug.Log("currentInitiator: " + _initiator);
    }

    public void ClearInitiator()
    {
        _initiator = null;
    }

    public void InitializeArena()
    {
        var tileMap = _initiator.GetTilemap();
        map = new Dictionary<Vector2Int, OverlayTile>();
        BoundsInt bounds = tileMap.cellBounds;
        Debug.Log(bounds.ToString());

        for (int y = bounds.min.y; y < bounds.max.y; y++)
        {
            for (int x = bounds.min.x; x < bounds.max.x; x++)
            {
                var tileLocation = new Vector3Int(x, y, 0);
                var tileKey = new Vector2Int(x, y);
                if (tileMap.HasTile(tileLocation) && !map.ContainsKey(tileKey))
                {
                    //Debug.Log(tileLocation);
                    var overlayTile = Instantiate(overlayTilePrefab, overlayContainer.transform);
                    //Debug.Log(tileMap.GetCellCenterWorld(tileLocation));
                    var cellWorldPosition = tileMap.GetCellCenterWorld(tileLocation);

                    overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 0.1f);
                    overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder;
                    overlayTile.gridLocation = tileLocation;
                    map.Add(tileKey, overlayTile);
                }
            }
        }
        //PositionPlayer(_characterStartingTile, _characterPrefab, _characterStartingOrientation);
        //PositionPlayer(_character1StartingTile, _character1Prefab, _character1StartingOrientation);

        //PositionEnemy(_enemyStartingTile, _enemyPrefab, _enemyStartingOrientation);
        //PositionEnemy(_enemy1StartingTile, _enemy1Prefab, _enemy1StartingOrientation);
        //Engine.Instance.TurnManager.SetNextCharacter();
        CursorController.Instance.ShowCursor();
        CursorController.Instance.ClearInRangeTiles();
        Engine.Instance.Player.HidePlayer();

        foreach (var initiator in _initiator.GetInitiators())
        {
            initiator.SetActive(false);
        }

        Engine.Instance.ChangeGameMode();
    }

    public void ClearArena()
    {
        UIManager.Instance._combatPanel.SetActive(false);

        foreach (var tile in GetAllTilesOnMap())
        {
            Destroy(tile.gameObject);
        }
        map = null;

        foreach (var character in Engine.Instance.TurnManager.GetCharactersInBattle())
        {
            Destroy(character);
        }
        Engine.Instance.TurnManager.ClearCharactersList();
        CursorController.Instance.HideCursor();
        Engine.Instance.Player.PlacePlayerAt(_initiator.GetPositionAfterFight());
        Engine.Instance.Player.ShowPlayer();

        foreach (var initiator in _initiator.GetInitiators())
        {
            initiator.SetActive(true);
            initiator.GetComponent<OffCombatEnemyController>().SetEnemyStartDirection();
        }

        ClearInitiator();
        UIManager.Instance.ClearCombatantQueue();
        Engine.Instance.ChangeGameMode();
    }

    public List<OverlayTile> GetAllTilesOnMap()
    {
        List<OverlayTile> allTiles = new List<OverlayTile>();
        foreach (var tile in map)
        {
            allTiles.Add(tile.Value);
        }

        return allTiles;
    }

    //TEMPORARY
    public void PositionPlayer(Vector2Int position, GameObject player, Vector2 orientaion)
    {
        map.TryGetValue(position, out OverlayTile tile);
        Debug.Log("tile: " + tile);
        GameObject character = Instantiate(player);

        Engine.Instance.UpdatePlayerGateway(character.GetComponent<TacticalPlayerGateway>());
        Engine.Instance.TurnManager.AddCharacterToTheList(character);

        character.transform.position = tile.transform.position;
        Engine.Instance.TacticalPlayer.Initialize();

        Engine.Instance.TacticalPlayer.SetDirection(orientaion);
        Engine.Instance.TacticalPlayer.SetActiveTile(tile);
        CursorController.Instance.SetMovementRange();
    }

    // merge into a universal function later
    public void PositionEnemy(Vector2Int position, GameObject player, Vector2 orientaion)
    {
        map.TryGetValue(position, out OverlayTile tile);
        //Debug.Log("tile: " + tile);
        GameObject character = Instantiate(player);
        Engine.Instance.TurnManager.AddCharacterToTheList(character);

        TacticalEnemyInfo enemy = character.GetComponent<TacticalEnemyInfo>();
        CharacterAnimation animation = character.GetComponentsInChildren<CharacterAnimation>()[0];
        Debug.Log("animation: " + animation);
        character.transform.position = tile.transform.position;

        animation.SetDirection(orientaion);
        enemy.SetActiveTile(tile);
    }
}
