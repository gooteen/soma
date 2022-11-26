﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGateway : MonoBehaviour
{
    [SerializeField] private CharacterAnimation _anim;
    private CharacterMovement _mov;

    private void Awake()
    {
        _mov = GetComponent<CharacterMovement>();
    }

    public bool IsFrozen()
    {
        return _mov.IsFrozen();
    }

    public void Freeze()
    {
        _mov.Freeze();
    }

    public void SetDirection(Vector2 direction)
    {
        _anim.SetDirection(direction);
    }

    public void SetStaticDirection(Vector2 direction)
    {
        _anim.SetStaticDirection(direction);
    }

    public void HidePlayer()
    {
        this.gameObject.SetActive(false);
    }

    public void ShowPlayer()
    {
        this.gameObject.SetActive(true);
    }

    public void PlacePlayerAt(Transform transform)
    {
        this.gameObject.transform.position = transform.position;
    }
}
