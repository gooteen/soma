using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public OverlayTile overlayTilePrefab;
    public GameObject overlayContainer;

    public Dictionary<Vector2Int, OverlayTile> map;

    public static MapManager Instance { get { return _instance; } }
    private static MapManager _instance;

    [SerializeField] private GameObject _characterPrefab;
    // temporary
    [SerializeField] private GameObject _enemyPrefab;

    [SerializeField] private Vector2Int _characterStartingTile;
    [SerializeField] private Vector2 _characterStartingOrientation;

    // temporary
    [SerializeField] private Vector2Int _enemyStartingTile;
    [SerializeField] private Vector2 _enemyStartingOrientation;

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
        var tileMap = gameObject.GetComponentInChildren<Tilemap>();
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

                    overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 2);
                    overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder;
                    overlayTile.gridLocation = tileLocation;
                    map.Add(tileKey, overlayTile);
                }
            }
        }
        PositionPlayer(_characterStartingTile);
        PositionEnemy(_enemyStartingTile);
    }

    // Update is called once per frame
    void Update()
    {
        if (Engine.Instance.InputManager.LeftMouseButtonPressed())
        {
            PrintMap();
        }
    }

    void PrintMap()
    {
        foreach (var pair in map)
        {
            Debug.Log($"key: {pair.Key} value : {pair.Value}");
        }
    }

    public void PositionPlayer(Vector2Int position)
    {
        map.TryGetValue(position, out OverlayTile tile);
        //Debug.Log("tile: " + tile);
        GameObject character = Instantiate(_characterPrefab);
        Engine.Instance.InitializeTacticalPlayer(character);

        character.transform.position = tile.transform.position;

        Engine.Instance.TacticalPlayer.SetDirection(_characterStartingOrientation);
        Engine.Instance.TacticalPlayer.SetActiveTile(tile);
        CursorController.Instance.SetInRangeTiles();
    }

    // merge into a universal function later
    public void PositionEnemy(Vector2Int position)
    {
        map.TryGetValue(position, out OverlayTile tile);
        //Debug.Log("tile: " + tile);
        GameObject character = Instantiate(_enemyPrefab);
        TacticalEnemyInfo enemy = character.GetComponent<TacticalEnemyInfo>();
        CharacterAnimation animation = character.GetComponentsInChildren<CharacterAnimation>()[0];
        Debug.Log("animation: " + animation);
        character.transform.position = tile.transform.position;

        animation.SetDirection(_enemyStartingOrientation);
        enemy.SetActiveTile(tile);
    }
}
