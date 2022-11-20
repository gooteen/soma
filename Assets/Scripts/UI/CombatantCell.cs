using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatantCell : MonoBehaviour
{
    [SerializeField] private GameObject _marker;

    public void HideMarker()
    {
        _marker.SetActive(false);
    }

    public void ShowMarker()
    {
        _marker.SetActive(true);
    }

    public void Pop()
    {
        Destroy(this.gameObject);
    }

}
