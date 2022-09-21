using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CombatInitiator : MonoBehaviour
{
    [SerializeField] private EnemyPosition[] _enemyPositions;
    [SerializeField] private Tilemap _targetTilemap;
    [SerializeField] private Squad _squad;
    [SerializeField] private GameObject[] _enemiesOnMap;

    [Header("Main character's starting position")]
    [SerializeField] private Vector2Int _playerStartingPosition;
    [SerializeField] private Vector2 _playerStartingOrientation;

    [Header("NE, NW, SE, SW")]
    [SerializeField] private string _playerLineDirection;

    public GameObject[] GetInitiators()
    {
        return _enemiesOnMap;
    }

    public Tilemap GetTilemap()
    {
        return _targetTilemap;
    }

    public Transform GetPositionAfterFight()
    {
        return gameObject.transform;
    }

    public void InitializeFight()
    {
        MapManager.Instance.SetInitiator(gameObject);
        MapManager.Instance.InitializeArena();

        PlacePlayers();
        PlaceEnemies();

        Engine.Instance.TurnManager.SetNextCharacter();
    }

    private void PlaceEnemies()
    {
        for (int i = 0; i < _enemyPositions.Length; i++)
        {
            for (int j = 0; j < _enemyPositions[i]._characterStartingTiles.Length; j++)
            {
                MapManager.Instance.PositionEnemy(_enemyPositions[i]._characterStartingTiles[j], _enemyPositions[i]._enemyPrefab, _enemyPositions[i]._characterStartingOrientation[j]);
            }
        }
    }

    private void PlacePlayers()
    {
        int _step = 0;
        Vector2Int _nextCharacterPosition = _playerStartingPosition;

        for (int i = 0; i < _squad._listOfSquadMembers.Length; i++)
        {
            if (_playerLineDirection == "NE")
            {
                _nextCharacterPosition = new Vector2Int(_playerStartingPosition.x + _step, _playerStartingPosition.y);
            }
            else if (_playerLineDirection == "NW")
            {
                _nextCharacterPosition = new Vector2Int(_playerStartingPosition.x, _playerStartingPosition.y + _step);
            }
            else if (_playerLineDirection == "SE")
            {
                _nextCharacterPosition = new Vector2Int(_playerStartingPosition.x, _playerStartingPosition.y - _step);
            }
            else if (_playerLineDirection == "SW")
            {
                _nextCharacterPosition = new Vector2Int(_playerStartingPosition.x - _step, _playerStartingPosition.y);
            }
            MapManager.Instance.PositionPlayer(_nextCharacterPosition, _squad._listOfSquadMembers[i], _playerStartingOrientation);
            _step = _step + 2;
        }
    }
}
