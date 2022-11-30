using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyMode { Still, Patrolling};

public class OffCombatEnemyController : MonoBehaviour
{
    [SerializeField] private CombatInitiator _initiator;
    [SerializeField] private CharacterAnimation _animation;
    [SerializeField] private Vector2 _dirAtStart;
    [SerializeField] private EnemyMode _mode;

    [SerializeField] private GameObject _canvas;
    [SerializeField] private Image _alertBar;
    [SerializeField] private bool _spotted;
    [SerializeField] private float _timeToCombat;
    [SerializeField] private float _currentTime;

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
        _currentTime = 0;
        _spotted = false;
        HideUI();
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
        if (_spotted)
        {
            ManageSpottedBar();
        }
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
                    _currentWayPointIndex++;
                    StartCoroutine("ToNextWaypoint");
                }
            }
        }
    }


    public void ManageSpottedBar()
    {
        float _timePassed = Time.time - _currentTime;
        _alertBar.fillAmount =_timePassed / _timeToCombat;
        if (_timePassed >= _timeToCombat)
        {
            _initiator.SetSpotted();
            _spotted = false;
            HideUI();
        }
    }

    public void HideUI()
    {
        _canvas.SetActive(false);
    }

    public void ShowUI()
    {
        _canvas.SetActive(true);
    }

    private void OnEnable()
    {
        if (_currentlyAtWaypoint)
        {
            StartCoroutine("ToNextWaypoint");
        }
    }

    private IEnumerator ToNextWaypoint()
    {
        if (_currentWayPointIndex == _wayPoints.Length)
        {
            _currentWayPointIndex = 0;
        }
        _currentWayPoint = _wayPoints[_currentWayPointIndex];
        yield return new WaitForSeconds(_pauseTime);
        _currentlyAtWaypoint = false;
        _character.Freeze();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _initiator.InitializeFight();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _spotted = true;
        ShowUI();
        _currentTime = Time.time;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _spotted = false;
        HideUI();
    }

    public void SetEnemyStartDirection()
    {
        _animation.SetStaticDirection(_dirAtStart);
    }
}
