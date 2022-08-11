﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalPlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private PlayerAnimation _anim;
    private PlayerInfo _info;

    private OverlayTile _destinationTile;
    private Transform _nextTile;
    private Vector2 _direction;

    void Awake()
    {
        _info = GetComponent<PlayerInfo>();
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

    public List<OverlayTile> MoveAlongPath(List<OverlayTile> path, OverlayTile destination)
    {
        _destinationTile = destination;
        _nextTile = path[0].transform;
        _direction = _nextTile.position - _info.GetActiveTile().transform.position;
        transform.position = Vector3.MoveTowards(transform.position, _nextTile.position, _speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, _nextTile.position) < 0.0001f)
        {
            _info.SetActiveTile(path[0]);
            if (_info.GetActiveTile() == _destinationTile)
            {
                Debug.Log("Prishel'");
                transform.position = _nextTile.position;
            }
            path.RemoveAt(0);
        }
        return path;
    }
}
