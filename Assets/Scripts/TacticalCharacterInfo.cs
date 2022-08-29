using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TacticalCharacterInfo : MonoBehaviour
{
    [SerializeField] private OverlayTile _activeTile;

    public void SetActiveTile(OverlayTile tile)
    {
        _activeTile = tile;
        // блокируем плитку на ход противкника
    }

    public OverlayTile GetActiveTile()
    {
        return _activeTile;
    }
}
