using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public static CursorController Instance { get { return _instance; } }
    private static CursorController _instance;

    [SerializeField] private LayerMask _gridMarkerLayerMask;
    [SerializeField] private LayerMask _enemyLayerMask;
    private TacticalCharacterInfo _currentFocusedEnemy;
    private OverlayTile _focusedTile;

    private PathFinder pathFinder;
    private RangeFinder rangeFinder;

    private List<OverlayTile> path = new List<OverlayTile>();
    private List<OverlayTile> inRangeTiles = new List<OverlayTile>();

    private OverlayTile _destinationTile;
    private bool _cursorLocked;
    private bool _isPaused;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        _cursorLocked = false;
        pathFinder = new PathFinder();
        rangeFinder = new RangeFinder();
    }

    void LateUpdate()
    {
        Debug.Log("isLocked: " + _cursorLocked);
        if (Engine.Instance.TacticalPlayer != null)
        {
            if (Engine.Instance.TacticalPlayer.GetActionPoints() > 0)
            {
                if (Engine.Instance._currentTacticalMode == TacticalMode.Movement)
                {
                    if (GetFocusedOnTile())
                    {
                        transform.position = new Vector3(_focusedTile.transform.position.x, _focusedTile.transform.position.y, _focusedTile.transform.position.z + 0.01f);
                        transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder = _focusedTile.GetComponent<SpriteRenderer>().sortingOrder;
                        if (Engine.Instance.InputManager.LeftMouseButtonPressed() && !_cursorLocked && !_isPaused)
                        {
                            _cursorLocked = true;
                            _destinationTile = _focusedTile;

                            path = pathFinder.FindPath(Engine.Instance.TacticalPlayer.GetActiveTile(), _destinationTile, inRangeTiles);
                            foreach (OverlayTile step in path)
                            {
                                Debug.Log($"PATH:{step.gridLocation}");
                            }

                        }
                    }
                }
                else
                {
                    if (GetFocusedOnEnemy())
                    {
                        if (Engine.Instance.InputManager.LeftMouseButtonPressed() && !_cursorLocked && !_isPaused)
                        {
                            if (inRangeTiles.Contains(_focusedTile))
                            {
                                if(Engine.Instance.TacticalPlayer.GetWeaponHitCost() <= Engine.Instance.TacticalPlayer.GetActionPoints())
                                {
                                    Engine.Instance.TacticalPlayer.OnWeaponUsed();
                                    Engine.Instance.TacticalPlayer.OnWeaponChosen();
                                    if (Engine.Instance.TacticalPlayer.GetActionPoints() == 0)
                                    {
                                        HideCursor();
                                    }
                                    if (Engine.Instance.TurnManager.EnemiesBeaten())
                                    {
                                        // экран победы
                                        MapManager.Instance.ClearArena();
                                    }
                                }
                            }
                        }
                    }
                }

                if (path.Count > 0)
                {
                    Debug.Log("Have a path!");

                    path = Engine.Instance.TacticalPlayer.MoveAlongPath(path, _destinationTile);
                    //MoveAlongPath();

                }
                else
                {
                    _cursorLocked = false;
                }
            }
            else
            {
                Debug.Log("next turn!!!");
                Engine.Instance.TurnManager.ToNextTurn();
            }
        }
    }

    public void Pause()
    {
        _isPaused = true;
    }

    public void UnPause()
    {
        _isPaused = false;
    }

    public void HideCursor()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

    }

    public void ShowCursor()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void ClearInRangeTiles()
    {
        inRangeTiles = new List<OverlayTile>();
    }

    public bool GetFocusedOnTile()
    {
        // The camera component tagged "MainCamera"
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Engine.Instance.InputManager.GetMousePosition());
        Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2d, Vector2.zero, Mathf.Infinity, _gridMarkerLayerMask, -Mathf.Infinity, Mathf.Infinity);
        if (hit.collider != null)
        {
            _focusedTile = hit.collider.gameObject.GetComponent<OverlayTile>();
            Debug.Log("Hit " + hit.collider.gameObject);
            return true;
        } else
        {
            return false;
        }
    }

    public TacticalCharacterInfo GetCurrentFocusedEnemy()
    {
        return _currentFocusedEnemy;
    }

    public bool GetFocusedOnEnemy()
    {
        // The camera component tagged "MainCamera"
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Engine.Instance.InputManager.GetMousePosition());
        Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2d, Vector2.zero, Mathf.Infinity, _enemyLayerMask, -Mathf.Infinity, Mathf.Infinity);
        if (hit.collider != null)
        {
            _currentFocusedEnemy = hit.collider.gameObject.GetComponent<TacticalCharacterInfo>();
            _focusedTile = _currentFocusedEnemy.GetActiveTile();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void HideTiles()
    {
        foreach (OverlayTile item in inRangeTiles)
        {
            item.HideTile();
        }
    }

    public void ShowTiles()
    {
        foreach (OverlayTile item in inRangeTiles)
        {
            item.ShowTile();
        }
    }

    public void SetMovementRange()
    {
        HideTiles();

        Debug.Log("Current ap: " + Engine.Instance.TacticalPlayer.GetActionPoints());
        inRangeTiles = rangeFinder.GetTilesInRange(Engine.Instance.TacticalPlayer.GetActiveTile(), Engine.Instance.TacticalPlayer.GetActionPoints(), true);

        if (Engine.Instance.TacticalPlayer.GetActionPoints() > 0)
        {
            ShowTiles();
        }
        Engine.Instance.ChangeTacticalMode(TacticalMode.Movement);
    }

    public void SetCombatRange(WeaponMode mode, int length)
    {
        foreach (OverlayTile item in inRangeTiles)
        {
            item.HideTile();
        }

        if (mode == WeaponMode.FilledRange)
        {
            inRangeTiles = rangeFinder.GetTilesInRange(Engine.Instance.TacticalPlayer.GetActiveTile(), length, false);
        }
        else if (mode == WeaponMode.FourLines)
        {
            inRangeTiles = rangeFinder.GetTilesInIntervalVer2(Engine.Instance.TacticalPlayer.GetActiveTile(), length, new List<string> { "NW", "SE", "SW", "NE" });
        } else if (mode == WeaponMode.SingleLine)
        {
            inRangeTiles = rangeFinder.GetTilesInInterval(Engine.Instance.TacticalPlayer.GetActiveTile(), length);
        }

        if (Engine.Instance.TacticalPlayer.GetActionPoints() > 0)
        {
            foreach (OverlayTile item in inRangeTiles)
            {
                item.ShowTile();
            }
        }
    }

    

    /*
    private void MoveAlongPath()
    {
        var step = 5 * Time.deltaTime;
        _player.transform.position = Vector3.MoveTowards(_player.transform.position, path[0].transform.position, step);
        if (Vector3.Distance(_player.transform.position, path[0].transform.position) < 0.0001f)
        {
            _player.transform.position = path[0].transform.position;
            _playerInfo.SetActiveTile(path[0]);
            path.RemoveAt(0);
        }
    }
    */
}
