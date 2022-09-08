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
    private bool cursorLocked;

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
        cursorLocked = false;
        pathFinder = new PathFinder();
        rangeFinder = new RangeFinder();
    }

    void LateUpdate()
    {
        if (Engine.Instance.mode == 0)
        {
            if (GetFocusedOnTile())
            {
                transform.position = new Vector3(_focusedTile.transform.position.x, _focusedTile.transform.position.y, _focusedTile.transform.position.z + 0.01f);
                transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder = _focusedTile.GetComponent<SpriteRenderer>().sortingOrder;
                if (Engine.Instance.InputManager.LeftMouseButtonPressed() && !cursorLocked)
                {
                    
                    cursorLocked = true;
                    _destinationTile = _focusedTile;

                    path = pathFinder.FindPath(Engine.Instance.TacticalPlayer.GetActiveTile(), _destinationTile, inRangeTiles);
                    foreach (OverlayTile step in path)
                    {
                        Debug.Log($"PATH:{step.gridLocation}");
                    }
                    
                }
            }
        } else
        {
            if (GetFocusedOnEnemy())
            {
                if (Engine.Instance.InputManager.LeftMouseButtonPressed() && !cursorLocked)
                {
                  if (inRangeTiles.Contains(_focusedTile))
                    {
                        _currentFocusedEnemy.TakeDamage(5);
                    } 
                }
            }
        }
        
        if (path.Count > 0)
        {
            Debug.Log("Have a path!");
           
            path = Engine.Instance.TacticalPlayer.MoveAlongPath(path, _destinationTile);
            //MoveAlongPath();
            
        } else
        {
            cursorLocked = false;
        }
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

    public void SetInRangeTiles()
    {
        foreach (OverlayTile item in inRangeTiles)
        {
            item.HideTile();
        }

        // feed the turn's remaining AP into the function
        if (Engine.Instance.mode == 0)
        {
            inRangeTiles = rangeFinder.GetTilesInRange(Engine.Instance.TacticalPlayer.GetActiveTile(), 3);

        } else if (Engine.Instance.mode == 1)
        {
            inRangeTiles = rangeFinder.GetTilesInRange(Engine.Instance.TacticalPlayer.GetActiveTile(), 1);
        } else
        {
            inRangeTiles = rangeFinder.GetTilesInIntervalVer2(Engine.Instance.TacticalPlayer.GetActiveTile(), 6, new List<string> { "NW", "SE", "SW", "NE" });
        }

        foreach (OverlayTile item in inRangeTiles)
        {
            item.ShowTile();
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
