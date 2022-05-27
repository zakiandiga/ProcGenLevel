using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{
    public static event Action<WallGenerator> OnMapCreationFinished;

    public void CreateWall(HashSet<Vector2Int> floorPositions, FloorVisualizer floorVisualizer)
    {
        HashSet<Vector2Int> basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.basicDirectionList);
        HashSet<Vector2Int> cornerWallPositions = FindWallsInDirections(floorPositions, Direction2D.diagonalDirectionList);
        //cornerWallPositions.ExceptWith(basicWallPositions); //exclude positions that's already in basicWallPositions

        CreateBasicWall(floorVisualizer, basicWallPositions, floorPositions);

        CreateCornerWall(floorVisualizer, cornerWallPositions, floorPositions);
    }

    private void CreateCornerWall(FloorVisualizer floorVisualizer, HashSet<Vector2Int> cornerWallPositions, HashSet<Vector2Int> floorPositions)
    {
        foreach(Vector2Int position in cornerWallPositions)
        {
            string adjecentBinaryValue = "";
            foreach(Vector2Int direction in Direction2D.eightDirectionalList)
            {
                Vector2Int adjecentPosition = position + direction;
                if (floorPositions.Contains(adjecentPosition))
                    adjecentBinaryValue += "1";

                else
                    adjecentBinaryValue += "0";
            }

            floorVisualizer.PaintSingleSpecialWall(position, adjecentBinaryValue);
        }

        OnMapCreationFinished?.Invoke(this);
    }

    private void CreateBasicWall(FloorVisualizer floorVisualizer, HashSet<Vector2Int> basicWallPositions, HashSet<Vector2Int> floorPositions)
    {
        foreach (Vector2Int position in basicWallPositions)
        {
            string adjecentBinaryValue = "";
            foreach(Vector2Int direction in Direction2D.basicDirectionList)
            {
                Vector2Int adjecentPosition = position + direction;
                if (floorPositions.Contains(adjecentPosition)) //if there's a floor on the adjecent position of this direction, add 1 to the binaryValue
                    adjecentBinaryValue += "1";

                else
                    adjecentBinaryValue += "0";
            }

            floorVisualizer.PaintSingleBasicWall(position, adjecentBinaryValue);
        }

        
    }

    private HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();

        foreach(Vector2Int position in floorPositions)
        {
            foreach(Vector2Int direction in directionList)
            {
                Vector2Int adjecentPosition = position + direction;
                if(!floorPositions.Contains(adjecentPosition))
                {
                    wallPositions.Add(adjecentPosition);
                }
            }
        }

        return wallPositions;
    }   

}
