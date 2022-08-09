using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayTile : MonoBehaviour
{
    private Color color;

    void Awake()
    {
        color = gameObject.GetComponent<SpriteRenderer>().color;
    }
   
    void Update()
    {
        if (InputManager.Instance.LeftMouseButtonPressed())
        {
            HideOverlay();
        }
    }

    public void ShowOverlay()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 1);
    }

    private void HideOverlay()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 0);
    }
}
