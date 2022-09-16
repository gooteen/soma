using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TacticalEnemyAI : MonoBehaviour
{
    [SerializeField] private List<GameObject> _playersInBattle;
    [SerializeField] private bool _inATurn;

    private TacticalCharacterInfo _info;
    private TacticalMovement _movement;

    private Dictionary<OverlayTile, GameObject> destinationToTarget;

    private List<OverlayTile> _currentPath;
    private OverlayTile _currentDestinationTile;
    private GameObject _currentTargetInReach;

    private bool _targetInReach;

    private PathFinder _pathFinder;
    private RangeFinder _rangeFinder;

    void Start()
    {
        _info = GetComponent<TacticalCharacterInfo>();
        _movement = GetComponent<TacticalMovement>();
        destinationToTarget = new Dictionary<OverlayTile, GameObject>();

        List<GameObject> _charactersInBattle = Engine.Instance.TurnManager.GetCharactersInBattle();
        foreach (GameObject character in _charactersInBattle)
        {
            if (character.tag != "EnemyTactical")
            {
                _playersInBattle.Add(character);
            }
        }
        _pathFinder = new PathFinder();
        _rangeFinder = new RangeFinder();
    }

    void Update()
    {
        if (_inATurn)
        {
            if(!_targetInReach)
            {
                if (_info.GetActionPoints() > 0)
                {
                    _currentPath = _movement.MoveAlongPath(_currentPath, _currentDestinationTile);
                } else
                {
                    _movement.DropDestinationTile();
                    FinishTurn();
                }
            } else
            {
                if (_info.GetActionPoints() > 0)
                {
                    _currentTargetInReach = destinationToTarget[_currentDestinationTile];
                    Hit();
                } else
                {
                    FinishTurn();
                }
            }
        }
    }

    public void SetTargetInReach()
    {
        _targetInReach = true;
    }

    public void StartTurn()
    {
        SetGoal();
        _inATurn = true;
    }

    private void Hit()
    {
        _currentTargetInReach.GetComponent<TacticalCharacterInfo>().TakeDamage(5);
        _info.TakeAwayActionPoints(1);
    }

    private void FinishTurn()
    {
        _inATurn = false;
        destinationToTarget.Clear();
        Engine.Instance.TurnManager.ToNextTurn();

    }

    private void SetGoal()
    {
        Dictionary<OverlayTile, int> tileManhattanDistances = new Dictionary<OverlayTile, int>();

        OverlayTile _closestTile;
        foreach (GameObject player in _playersInBattle)
        {
            OverlayTile _playerCurrentTile = player.GetComponent<TacticalCharacterInfo>().GetActiveTile();
            List<OverlayTile> _playerTilesInRange = _rangeFinder.GetTilesInRange(_playerCurrentTile, 1, true);
            List<OverlayTile> _playerNeighbourTiles = _pathFinder.GetNeighbourTiles(_playerCurrentTile, _playerTilesInRange, new List<string> { "NW", "SE", "SW", "NE" }, true);
            foreach (OverlayTile tile in _playerNeighbourTiles)
            {
                int tileManhattanDistance =_pathFinder.GetManhattanDistance(_info.GetActiveTile(), tile);
                if (!tileManhattanDistances.ContainsKey(tile))
                {
                    tileManhattanDistances.Add(tile, tileManhattanDistance);
                    destinationToTarget.Add(tile, player);
                }
            }
        }
        _closestTile = tileManhattanDistances.OrderBy(x => x.Value).First().Key;
        _currentDestinationTile = _closestTile;
        if (_closestTile != _info.GetActiveTile())
        {
            _targetInReach = false;
            _currentPath = _pathFinder.FindPath(_info.GetActiveTile(), _closestTile, MapManager.Instance.GetAllTilesOnMap());
        } else
        {
            _targetInReach = true;
        }
    }
}
