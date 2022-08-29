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


    public OverlayTile previous;

    public Vector3Int gridLocation;

    private bool isBlocked;

    private Color color;

    void Awake()
    {
        color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public void BlockTile()
    {
        isBlocked = true;
    }

    public void UnlockTile()
    {
        isBlocked = false;
    }

    public bool IsBlocked()
    {
        return isBlocked;
    }

    public void ShowTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 1);
    }

    public void HideTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 0);
    }
}
