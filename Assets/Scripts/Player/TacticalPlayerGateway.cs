using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalPlayerGateway : MonoBehaviour
{
    [SerializeField] private PlayerInfo _playerInfo;
    private TacticalPlayerMovement _tacticalPlayerMovement;
    [SerializeField] private PlayerAnimation _playerAnimation;

    private void Awake()
    {
        _playerInfo = GetComponent<PlayerInfo>();
        _tacticalPlayerMovement = GetComponent<TacticalPlayerMovement>();
    }

    public List<OverlayTile> MoveAlongPath(List<OverlayTile> path, OverlayTile destination)
    {
        return _tacticalPlayerMovement.MoveAlongPath(path, destination);
    }

    public void SetDirection(Vector2 direction)
    {
        _playerAnimation.SetDirection(direction);
    }

    public void SetActiveTile(OverlayTile tile)
    {
        _playerInfo.SetActiveTile(tile);
    }

    public OverlayTile GetActiveTile()
    {
        return _playerInfo.GetActiveTile();
    }
}
