using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RangeFinder
{
    private PathFinder _pathFinder;

    public List<OverlayTile> GetTilesInRange(OverlayTile startingTile, int range)
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
                surroundingTiles.AddRange(_pathFinder.GetNeighbourTiles(item));

            }
            inRangeTiles.AddRange(surroundingTiles);
            tileForPreviousStep = surroundingTiles.Distinct().ToList();
            stepCount++;
        }
        return inRangeTiles.Distinct().ToList();
    }
}
