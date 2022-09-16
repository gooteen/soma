using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RangeFinder
{
    private PathFinder _pathFinder;

    public List<OverlayTile> GetTilesInRange(OverlayTile startingTile, int range, bool needToExcludeBlockedTiles)
    {
        _pathFinder = new PathFinder();
        List<OverlayTile> inRangeTiles = new List<OverlayTile>();
        int stepCount = 0;

        inRangeTiles.Add(startingTile);

        List<OverlayTile> tileForPreviousStep = new List<OverlayTile>();
        tileForPreviousStep.Add(startingTile);

        while (stepCount < range)
        {
            List<OverlayTile> surroundingTiles = new List<OverlayTile>();

            foreach (OverlayTile item in tileForPreviousStep)
            {
                surroundingTiles.AddRange(_pathFinder.GetNeighbourTiles(item, new List<OverlayTile>(), new List<string> { "NW", "SE", "SW", "NE" }, needToExcludeBlockedTiles));

            }
            inRangeTiles.AddRange(surroundingTiles);
            tileForPreviousStep = surroundingTiles.Distinct().ToList();
            stepCount++;
        }
        return inRangeTiles.Distinct().ToList();
    }

    public List<OverlayTile> GetTilesInInterval(OverlayTile startingTile, int range)
    {
        _pathFinder = new PathFinder();
        List<OverlayTile> inRangeTiles = new List<OverlayTile>();
        int stepCount = 0;
        string _direction = Engine.Instance.TacticalPlayer.GetCurrentStaticDirection().Split(' ')[1];
        Debug.Log("dir " + _direction);
        inRangeTiles.Add(startingTile);

        List<OverlayTile> tileForPreviousStep = new List<OverlayTile>();
        tileForPreviousStep.Add(startingTile);

        while (stepCount < range)
        {
            List<OverlayTile> surroundingTiles = new List<OverlayTile>();

            foreach (OverlayTile item in tileForPreviousStep)
            {
                surroundingTiles.AddRange(_pathFinder.GetNeighbourTiles(item, new List<OverlayTile>(), new List<string> { _direction }, false));

            }
            inRangeTiles.AddRange(surroundingTiles);
            tileForPreviousStep = surroundingTiles.Distinct().ToList();
            stepCount++;
        }
        return inRangeTiles.Distinct().ToList();
    }

    public List<OverlayTile> GetTilesInIntervalVer2(OverlayTile startingTile, int range, List<string> directions)
    {
        _pathFinder = new PathFinder();
        List<OverlayTile> inRangeTiles = new List<OverlayTile>();

        inRangeTiles.Add(startingTile);

        foreach (string direction in directions)
        {
            int stepCount = 0;
            List<OverlayTile> tileForPreviousStep = new List<OverlayTile>();
            tileForPreviousStep.Add(startingTile);
            while (stepCount < range)
            {
                List<OverlayTile> surroundingTiles = new List<OverlayTile>();

                foreach (OverlayTile item in tileForPreviousStep)
                {
                    surroundingTiles.AddRange(_pathFinder.GetNeighbourTiles(item, new List<OverlayTile>(), new List<string> {direction}, false));

                }
                inRangeTiles.AddRange(surroundingTiles);
                tileForPreviousStep = surroundingTiles.Distinct().ToList();
                stepCount++;
            }
        }
        
        return inRangeTiles.Distinct().ToList();
    }

    private List<OverlayTile> RemoveBlockedTilesFromList(List<OverlayTile> tiles)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].IsBlocked())
            {
                tiles.RemoveAt(i);
            }
        }
        return tiles;
    }

}
