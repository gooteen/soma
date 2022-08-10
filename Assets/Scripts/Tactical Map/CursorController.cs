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

    private List<OverlayTile> path = new List<OverlayTile>();

    void Start()
    {
        pathFinder = new PathFinder();
    }

    void LateUpdate()
    {
        //Debug.Log(GetFocusedOnTile());
        
        if (GetFocusedOnTile())
        {
            transform.position = _focusedTile.transform.position;
            transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder = _focusedTile.GetComponent<SpriteRenderer>().sortingOrder;
            if(InputManager.Instance.LeftMouseButtonPressed())
            {
                _focusedTile.GetComponent<OverlayTile>().ShowOverlay();

                _player = GameObject.Find("CharacterTactical(Clone)");

                if (_player != null)
                {
                    _playerInfo = _player.GetComponent<PlayerInfo>();
                } else
                {
                    Debug.Log("No player");

                }

                if (_playerInfo != null)
                {
                    path = pathFinder.FindPath(_playerInfo.GetActiveTile(), _focusedTile);
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
            MoveAlongPath();
        }
    }

    public bool GetFocusedOnTile()
    {
        // The camera component tagged "MainCamera"
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(InputManager.Instance.GetMousePosition());
        Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2d, Vector2.zero, Mathf.Infinity, _layerMask, -Mathf.Infinity, Mathf.Infinity);
        if (hit.collider != null)
        {
            _focusedTile = hit.collider.gameObject.GetComponent<OverlayTile>();
            Debug.Log("Hit " + hit.collider.gameObject);
            Debug.DrawRay(mousePos2d, Vector2.zero * 10);
            return true;
        } else
        {
            return false;
        }
    }

    private void MoveAlongPath()
    {
        var step = 5 * Time.deltaTime;
        _player.transform.position = Vector2.MoveTowards(_player.transform.position, path[0].transform.position, step);
        if (Vector2.Distance(_player.transform.position, path[0].transform.position) < 0.0001f)
        {
            _player.transform.position = path[0].transform.position;
            _playerInfo.SetActiveTile(path[0]);
            path.RemoveAt(0);
        }
    }
}
