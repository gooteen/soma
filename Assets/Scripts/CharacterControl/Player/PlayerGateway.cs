using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGateway : MonoBehaviour
{
    [SerializeField] private CharacterAnimation _anim;
    private CharacterMovement _mov;
    private PlayerInfo _info;

    private void Awake()
    {
        _mov = GetComponent<CharacterMovement>();
        _info = GetComponent<PlayerInfo>();
    }

    public void RestoreHealth(float healthToRestore)
    {
        _info.RestoreHealth(healthToRestore);
    }

    public void LockItemPickup()
    {
        _mov.LockItemPickup();
    }

    public void UnlockItemPickup()
    {
        _mov.UnlockItemPickup();
    }

    public bool CanPickUpItems()
    {
        return _mov.CanPickUpItems();
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
