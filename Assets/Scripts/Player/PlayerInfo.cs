using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private OverlayTile _activeTile;

    public void SetActiveTile(OverlayTile tile)
    {
        _activeTile = tile;
    }

    public OverlayTile GetActiveTile()
    {
        return _activeTile;
    }
}
