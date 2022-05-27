using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomWalkGen : AbstractMapGen
{

    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk(startPos);

        floorVisualizer.ClearGeneratedTiles();
        floorVisualizer.PaintFloor(floorPositions);

        wallGenerator.CreateWall(floorPositions, floorVisualizer);
    }

    public HashSet<Vector2Int> RunRandomWalk2(Vector2Int position)
    {
        var currentPos = position;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

        for (int i = 0; i < iteration; i++)
        {
            var path = ProcGen.RandomWalk(currentPos, walkLength);
            floorPositions.UnionWith(path);

            if (startRandomlyEachIteration)
                currentPos = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
        }

        return floorPositions;
    }

    private void Start()
    {
        floorVisualizer = GetComponent<FloorVisualizer>();
    }
}
