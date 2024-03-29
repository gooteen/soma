﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder
{
    public List<OverlayTile> FindPath(OverlayTile start, OverlayTile end, List<OverlayTile> searchableTiles)
    {
        List<OverlayTile> openList = new List<OverlayTile>();
        List<OverlayTile> closedList = new List<OverlayTile>();

        openList.Add(start);

        while (openList.Count > 0)
        {
            OverlayTile currentOverlayTile = openList.OrderBy(x => x.F).First();

            openList.Remove(currentOverlayTile);
            closedList.Add(currentOverlayTile);

            if (currentOverlayTile == end)
            {
                return GetFinishedList(start, end);
            }

            var neighbourTiles = GetNeighbourTiles(currentOverlayTile, searchableTiles, new List<string> { "NW", "SE", "SW", "NE" }, true);

            foreach (var neighbour in neighbourTiles)
            {
                if (neighbour.IsBlocked() || closedList.Contains(neighbour))
                {
                    continue;
                }

                neighbour.G = GetManhattanDistance(start, neighbour);
                neighbour.H = GetManhattanDistance(end, neighbour);

                neighbour.previous = currentOverlayTile;

                if (!openList.Contains(neighbour))
                {
                    openList.Add(neighbour);
                }
            }
        }
        return new List<OverlayTile>();
    }

    public int GetManhattanDistance(OverlayTile start, OverlayTile neighbour)
    {
        return Mathf.Abs(start.gridLocation.x - neighbour.gridLocation.x) + Mathf.Abs(start.gridLocation.y - neighbour.gridLocation.y);
    }

    public List<OverlayTile> GetNeighbourTiles(OverlayTile currentOverlayTile, List<OverlayTile> searchableTiles, List<string> modes, bool needToExcludeBlockedTiles)
    {
        Dictionary<Vector2Int, OverlayTile> map = MapManager.Instance.map;
        Dictionary<Vector2Int, OverlayTile> tilesToSearch = new Dictionary<Vector2Int, OverlayTile>();

        if (searchableTiles.Count > 0)
        {
            foreach (OverlayTile item in searchableTiles)
            {
                tilesToSearch.Add(new Vector2Int(item.gridLocation.x, item.gridLocation.y), item);
            }
        } else
        {
            tilesToSearch = map;
        }

        List<OverlayTile> neighbours = new List<OverlayTile>();

        if (modes.Contains("NW"))
        {
            Vector2Int locationToCheckTop = new Vector2Int(
           currentOverlayTile.gridLocation.x,
           currentOverlayTile.gridLocation.y + 1
           );

            if (tilesToSearch.ContainsKey(locationToCheckTop) && !(needToExcludeBlockedTiles && tilesToSearch[locationToCheckTop].IsBlocked()))
            {
                neighbours.Add(tilesToSearch[locationToCheckTop]);
            }
        }

        if (modes.Contains("SE"))
        {
            Vector2Int locationToCheckBottom = new Vector2Int(
        currentOverlayTile.gridLocation.x,
        currentOverlayTile.gridLocation.y - 1
        );

            if (tilesToSearch.ContainsKey(locationToCheckBottom) && !(needToExcludeBlockedTiles && tilesToSearch[locationToCheckBottom].IsBlocked()))
            {
                neighbours.Add(tilesToSearch[locationToCheckBottom]);
            }
        }
        if (modes.Contains("NE"))
        {
            Vector2Int locationToCheckRight = new Vector2Int(
        currentOverlayTile.gridLocation.x + 1,
        currentOverlayTile.gridLocation.y
        );

            if (tilesToSearch.ContainsKey(locationToCheckRight) && !(needToExcludeBlockedTiles && tilesToSearch[locationToCheckRight].IsBlocked()))
            {
                neighbours.Add(tilesToSearch[locationToCheckRight]);
            }
        }
        if (modes.Contains("SW"))
        {
            Vector2Int locationToCheckLeft = new Vector2Int(
            currentOverlayTile.gridLocation.x - 1,
            currentOverlayTile.gridLocation.y
            );

            if (tilesToSearch.ContainsKey(locationToCheckLeft) && !(needToExcludeBlockedTiles && tilesToSearch[locationToCheckLeft].IsBlocked()))
            {
                neighbours.Add(tilesToSearch[locationToCheckLeft]);
            }
        }
        return neighbours;
    }

    private List<OverlayTile> GetFinishedList(OverlayTile start, OverlayTile end)
    {
        List<OverlayTile> finishedList = new List<OverlayTile>();
        OverlayTile currentTile = end;

        while (currentTile != start)
        {
            finishedList.Add(currentTile);
            currentTile = currentTile.previous;
        }

        finishedList.Reverse();

        return finishedList;
    }
}
