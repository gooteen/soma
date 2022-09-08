using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] private float _staticThreshold;
    [SerializeField] private int _numberOfSlices;

    // 8-directional movement

    private string[] _staticDirections = { "Static N", "Static NW", "Static W", "Static SW", "Static S", "Static SE", "Static E", "Static NE" };
    private string[] _runDirections = { "Run N", "Run NW", "Run W", "Run SW", "Run S", "Run SE", "Run E", "Run NE" };

    // 4-directional movement
    /*
    private string[] _staticDirections = { "Static N", "Static W", "Static S", "Static E"};
    private string[] _runDirections = { "Run N", "Run W", "Run S", "Run E"};
    */

    private Animator _anim;

    private int _lastDirection;


    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }
 
    public void SetDirection(Vector2 direction)
    {
        string[] _directionArray = null;
        if (direction.magnitude < _staticThreshold)
        {
            _directionArray = _staticDirections;
        }
        else
        {
            _directionArray = _runDirections;
            _lastDirection = DirectionToIndex(direction);
        }
        _anim.Play(_directionArray[_lastDirection]);
    }

    public string GetCurrentStaticDirection()
    {
        return _staticDirections[_lastDirection];
    }

    private int DirectionToIndex(Vector2 direction)
    {
        float step = 360 / _numberOfSlices;
        float offset = step / 2;

        float angle = Vector2.SignedAngle(Vector2.up, direction.normalized);

        angle += offset;
        if(angle < 0)
        {
            angle += 360;
        }

        float stepCount = angle / step;
        return Mathf.FloorToInt(stepCount);
    }
}
