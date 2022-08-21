using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayTile : MonoBehaviour
{
    // the number of cells from the starting position
    public int G;
    // the number of cells until the end position
    public int H; 

    public int F { get { return G + H; } }

    public bool isBlocked;

    public OverlayTile previous;

    public Vector3Int gridLocation;

    private Color color;

    void Awake()
    {
        color = gameObject.GetComponent<SpriteRenderer>().color;
    }
   
    void Update()
    {
        if (Engine.Instance.InputManager.LeftMouseButtonPressed())
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
