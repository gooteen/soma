using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        Engine.Instance.Player.UnlockItemPickup();
    }

    void OnTriggerExit2D(Collider2D col)
    {
        Engine.Instance.Player.LockItemPickup();
    }
}
