using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSortingSwitch : MonoBehaviour
{
    [SerializeField] private GameObject _elevationTilemap;
    private TilemapRenderer _renderer;

    void Start()
    {
        _renderer = _elevationTilemap.GetComponent<TilemapRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enter");
        _renderer.sortingOrder = 1;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        _renderer.sortingOrder = 2;
    }
}
