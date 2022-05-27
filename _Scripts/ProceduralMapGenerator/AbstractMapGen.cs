using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AbstractMapGen : MonoBehaviour
{    
    [SerializeField] protected FloorVisualizer floorVisualizer;
    [SerializeField] protected WallGenerator wallGenerator;
    [SerializeField] protected Vector2Int startPos = Vector2Int.zero;

    public RandomWalkData walkData;
    protected int tileSize => walkData.tileSize;
    protected int iteration => walkData.iteration;
    protected int walkLength => walkData.walkLength;
    protected bool startRandomlyEachIteration => walkData.startRandomEachIteration;

    public HashSet<Vector2Int> RunRandomWalk(Vector2Int position)
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

    public void GenerateMap()
    {
        floorVisualizer.ClearGeneratedTiles();
        RunProceduralGeneration();
    }

    public void ClearMap()
    {
        floorVisualizer.ClearGeneratedTiles();
    }

    protected abstract void RunProceduralGeneration();
}
