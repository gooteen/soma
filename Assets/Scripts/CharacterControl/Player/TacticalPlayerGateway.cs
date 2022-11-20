using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalPlayerGateway : MonoBehaviour
{
    [SerializeField] private TacticalPlayerInfo _playerInfo;
    private TacticalMovement _tacticalPlayerMovement;
    [SerializeField] private CharacterAnimation _playerAnimation;

    private void Awake()
    {
        _playerInfo = GetComponent<TacticalPlayerInfo>();
        _tacticalPlayerMovement = GetComponent<TacticalMovement>();
    }

    public float GetHealthRatio()
    {
        return _playerInfo.GetHealthRatio();
    }

    public int GetWeaponHitCost()
    {
        return _playerInfo.GetHitCost();
    }

    public void OnWeaponChosen()
    {
        _playerInfo.OnWeaponChosen();
    }

    public void OnWeaponUsed()
    {
        _playerInfo.OnWeaponUsed();
    }

    public void Initialize()
    {
        _playerInfo.Initialize();
    }

    public int GetActionPoints()
    {
        return _playerInfo.GetActionPoints();
    }

    public void TakeAwayActionPoints(int points)
    {
        _playerInfo.TakeAwayActionPoints(points);
    }

    public List<OverlayTile> MoveAlongPath(List<OverlayTile> path, OverlayTile destination)
    {
        return _tacticalPlayerMovement.MoveAlongPath(path, destination);
    }

    public string GetCurrentStaticDirection()
    {
        return _playerAnimation.GetCurrentStaticDirection();
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
