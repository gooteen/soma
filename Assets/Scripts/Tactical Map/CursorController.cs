using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    private OverlayTile _focusedTile;

    private PathFinder pathFinder;

    private GameObject _player;
    private PlayerInfo _playerInfo;
    private TacticalPlayerMovement _playerMovement;

    private List<OverlayTile> path = new List<OverlayTile>();
    private OverlayTile _destinationTile;
    private bool cursorLocked;

    void Start()
    {
        cursorLocked = false;
        pathFinder = new PathFinder();
    }

    void LateUpdate()
    {
        if (GetFocusedOnTile())
        {
            transform.position = _focusedTile.transform.position;
            transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder = _focusedTile.GetComponent<SpriteRenderer>().sortingOrder;
            if(InputManager.Instance.LeftMouseButtonPressed() && !cursorLocked)
            {
                cursorLocked = true;
                _destinationTile = _focusedTile;
                _destinationTile.GetComponent<OverlayTile>().ShowOverlay();

                _player = GameObject.FindWithTag("PlayerTactical");

                if (_player != null)
                {
                    _playerInfo = _player.GetComponent<PlayerInfo>();
                    _playerMovement = _player.GetComponent<TacticalPlayerMovement>();
                } else
                {
                    Debug.Log("No player");
                }

                if (_playerInfo != null)
                {
                    path = pathFinder.FindPath(_playerInfo.GetActiveTile(), _destinationTile);
                    foreach (OverlayTile step in path)
                    {
                        Debug.Log($"PATH:{step.gridLocation}");
                    }
                } else
                {
                    Debug.Log("No player info");
                }
            }
        }
        if (path.Count > 0)
        {
            Debug.Log("Have a path!");
            if (_playerMovement != null)
            {
                path = _playerMovement.MoveAlongPath(path, _destinationTile);
                //MoveAlongPath();
            } else
            {
                Debug.Log("No player movement");
            }
        } else
        {
            cursorLocked = false;
        }
    }

    public bool GetFocusedOnTile()
    {
        if (!cursorLocked)
        {

        }
        // The camera component tagged "MainCamera"
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(InputManager.Instance.GetMousePosition());
        Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2d, Vector2.zero, Mathf.Infinity, _layerMask, -Mathf.Infinity, Mathf.Infinity);
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

    // Transfer to a separate player movement script
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
}
