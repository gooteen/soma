using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private CharacterAnimation _anim;
    [SerializeField] private int ActionPointsPerStep;
    private TacticalCharacterInfo _info;

    private OverlayTile _destinationTile;
    private Transform _nextTile;
    private Vector2 _direction;

    void Awake()
    {
        _info = GetComponent<TacticalCharacterInfo>();
    }

    void Update()
    {
        if (_destinationTile == null)
        {
            _anim.SetDirection(new Vector2(0, 0));
        } else
        {
            if (_info.GetActiveTile() != _destinationTile)
            {
                _anim.SetDirection(_direction.normalized);
            }
            else
            {
                _anim.SetDirection(new Vector2(0, 0));
            }
        }
    }

    public void DropDestinationTile()
    {
        _destinationTile = null;
    }

    public List<OverlayTile> MoveAlongPath(List<OverlayTile> path, OverlayTile destination)
    {
        _destinationTile = destination;
        _nextTile = path[0].transform;
        _direction = _nextTile.position - _info.GetActiveTile().transform.position;
        transform.position = Vector3.MoveTowards(transform.position, _nextTile.position, _speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, _nextTile.position) < 0.0001f)
        {
            _info.TakeAwayActionPoints(ActionPointsPerStep);
            _info.SetActiveTile(path[0]);
            if (_info.GetActiveTile() == _destinationTile)
            {
                transform.position = _nextTile.position;
                if (gameObject.tag != "EnemyTactical")
                {
                    CursorController.Instance.SetInRangeTiles();
                } else
                {
                    gameObject.GetComponent<TacticalEnemyAI>().SetTargetInReach();
                }
            }
            path.RemoveAt(0);
        }
        return path;
    }

    
}
