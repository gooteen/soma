using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public enum EnemyMode { Still, Patrolling};

public class OffCombatEnemyController : MonoBehaviour
{
    [SerializeField] private CombatInitiator _initiator;
    [SerializeField] private CharacterAnimation _animation;
    [SerializeField] private Vector2 _dirAtStart;
    [SerializeField] private EnemyMode _mode;

    [Header("Settings for the Patrolling enemy mode")]
    [SerializeField] private Transform[] _wayPoints;
    [SerializeField] private float _pauseTime;
    [SerializeField] private int _currentWayPointIndex;
    [SerializeField] private bool _currentlyAtWaypoint;
    [SerializeField] private Transform _currentWayPoint;
    [SerializeField] private float _offset;
    private CharacterMovement _character;
    
    void Start()
    {
        _character = GetComponent<CharacterMovement>();
        SetEnemyStartDirection();
        if (_mode == EnemyMode.Patrolling)
        {
            _currentWayPointIndex = 0;
            _currentWayPoint = _wayPoints[_currentWayPointIndex];
            transform.position = _currentWayPoint.position;
            _currentlyAtWaypoint = true;
            _character.Freeze();
            StartCoroutine("ToNextWaypoint");
        }
        else
        {
            _character.Freeze();
        }
    }

    void Update()
    {
        if (_mode == EnemyMode.Patrolling)
        {
            if (!_currentlyAtWaypoint)
            {
                if (Vector2.Distance(transform.position, _currentWayPoint.position) >= _offset || Vector2.Distance(transform.position, _currentWayPoint.position) <= -_offset)
                {
                    Debug.Log("moving");
                    _character.Move(_currentWayPoint.position - transform.position);
                } else
                {
                    Debug.Log("reached");
                    _character.Freeze();
                    _currentlyAtWaypoint = true;
                    transform.position = _currentWayPoint.position;
                    StartCoroutine("ToNextWaypoint");
                }
            }
        }
    }

    private IEnumerator ToNextWaypoint()
    {
        _currentWayPointIndex++;
        if (_currentWayPointIndex == _wayPoints.Length)
        {
            _currentWayPointIndex = 0;
        }
        _currentWayPoint = _wayPoints[_currentWayPointIndex];
        yield return new WaitForSeconds(_pauseTime);
        _currentlyAtWaypoint = false;
        _character.Freeze();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Initiated" + collision.gameObject);
        _initiator.InitializeFight();
    }

    public void SetEnemyStartDirection()
    {
        _animation.SetStaticDirection(_dirAtStart);
    }
}
