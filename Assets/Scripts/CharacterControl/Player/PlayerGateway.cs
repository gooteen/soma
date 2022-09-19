using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGateway : MonoBehaviour
{
    public void HidePlayer()
    {
        this.gameObject.SetActive(false);
    }

    public void ShowPlayer()
    {
        this.gameObject.SetActive(true);
    }
}
