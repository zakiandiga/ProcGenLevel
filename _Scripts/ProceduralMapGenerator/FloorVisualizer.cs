using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorVisualizer : MonoBehaviour
{
    public int SizeMultiplier => sizeMultiplier;
    [SerializeField] private int sizeMultiplier = 1;
    [SerializeField] private GameObject floorTile;

    [SerializeField] private GameObject wallNorth, wallSouth, wallEast, wallWest;
    [SerializeField] private GameObject innerCornerNE, innerCornerSE, innerCornerSW, innerCornerNW;
    [SerializeField] private GameObject edgeNE, edgeSE, edgeSW, edgeNW;
    [SerializeField] private GameObject holeFiller;
    [SerializeField] private Transform floorParent;
    [SerializeField] private Transform wallParent;

    [SerializeField] private GameObject pathwayTile;
    [SerializeField] private Transform pathwayParent;

    private List<GameObject> generatedFloors = new List<GameObject>();
    //private List<GameObject> generatedBasicWalls = new List<GameObject>();
    private List<GameObject> generatedSpecialWalls = new List<GameObject>();
    private List<GameObject> generatedPathway = new List<GameObject>();
    private Dictionary<Vector2Int, GameObject> basicWalls = new Dictionary<Vector2Int, GameObject>();

    internal void PaintSingleBasicWall(Vector2Int position, string binaryValue)
    {               
        int binaryInt = Convert.ToInt32(binaryValue, 2);
        GameObject wallToPlace = GetBasicWallToPlace(binaryInt);    

        if (wallToPlace != null)
            InstantiateBasicWall(wallToPlace, position, wallParent);
    }

    private GameObject GetBasicWallToPlace(int binaryInt)
    {
        GameObject wallToPlace = null;

        if (WallTypesHelper.wallNorth.Contains(binaryInt))
        {
            wallToPlace = wallNorth;
        }

        else if (WallTypesHelper.wallSouth.Contains(binaryInt))
        {
            wallToPlace = wallSouth;
        }

        else if (WallTypesHelper.wallEast.Contains(binaryInt))
        {
            wallToPlace = wallEast;
        }

        else if (WallTypesHelper.wallWest.Contains(binaryInt))
        {
            wallToPlace = wallWest;
        }

        return wallToPlace;
    }

    private void InstantiateBasicWall(GameObject wall, Vector2Int position, Transform parent)
    {
        //generatedBasicWalls.Add(Instantiate(wall, new Vector3(position.x, 0, position.y), Quaternion.identity, parent));
        basicWalls.Add(position, Instantiate(wall, new Vector3(position.x, 0, position.y)* sizeMultiplier, Quaternion.identity, parent));
    }

    public void PaintSingleSpecialWall(Vector2Int position, string adjecentBinaryValue)
    {
        int binaryInt = Convert.ToInt32(adjecentBinaryValue, 2);
        GameObject wallToPlace = GetSpecialWallToPaint(binaryInt);
        

        if (!basicWalls.ContainsKey(position))
        {
            if (wallToPlace != null) //if wallToPlace assigned according to the helper AND there's no basic walls on that position
                InstantiateCornerWall(wallToPlace, position, wallParent);
            //else
            //    Debug.Log("Hole found at: " + position);
        }

    }

    private GameObject GetSpecialWallToPaint(int binaryInt)
    {
        GameObject wallToPlace = null;

        //Inner corner
        if (WallTypesHelper.wallInnerCornerSouthWest.Contains(binaryInt))
        {
            wallToPlace = innerCornerSW;
        }
        else if (WallTypesHelper.wallInnerCornerSouthEast.Contains(binaryInt))
        {
            wallToPlace = innerCornerSE;
        }
        else if (WallTypesHelper.wallInnerCornerNorthWest.Contains(binaryInt))
        {
            wallToPlace = innerCornerNW;
        }
        else if (WallTypesHelper.wallInnerCornerNorthEast.Contains(binaryInt))
        {
            wallToPlace = innerCornerNE;
        }

        //Edge

        else if (WallTypesHelper.wallDiagonalCornerNorthEast.Contains(binaryInt))
        {
            wallToPlace = edgeNE;
        }
        else if (WallTypesHelper.wallDiagonalCornerNorthWest.Contains(binaryInt))
        {
            wallToPlace = edgeNW;
        }
        else if (WallTypesHelper.wallDiagonalCornerSouthEast.Contains(binaryInt))
        {
            wallToPlace = edgeSW;
        }
        else if (WallTypesHelper.wallDiagonalCornerSouthWest.Contains(binaryInt))
        {
            wallToPlace = edgeSE;
        }

        //Hole
        else if (WallTypesHelper.wallFullEightDirections.Contains(binaryInt))
        {
            wallToPlace = holeFiller;
        }

        return wallToPlace;
    }

    private void InstantiateCornerWall(GameObject wall, Vector2Int position, Transform parent)
    {
        GameObject wallToInstantiate = wall;
        generatedSpecialWalls.Add(Instantiate(wallToInstantiate, new Vector3(position.x, 0, position.y) * sizeMultiplier, Quaternion.identity, parent));
    }


    public void PaintFloor(IEnumerable<Vector2Int> floorPositions)
    {
        InstantiateFloor(floorTile, floorPositions, floorParent);
    }

    private void InstantiateFloor(GameObject tile, IEnumerable<Vector2Int> positions, Transform tileParent)
    {        
        foreach (var position in positions)
        {
            generatedFloors.Add(Instantiate(tile, new Vector3(position.x, 0, position.y) * sizeMultiplier, Quaternion.identity, tileParent));
        }
    }

    public void PaintPathways(IEnumerable<Vector2Int> pathwayPositions)
    {
        InstantiatePathway(pathwayTile, pathwayPositions, pathwayParent);
    }

    private void InstantiatePathway(GameObject pathwayTile, IEnumerable<Vector2Int> pathwayPositions, Transform pathwayParent)
    {
        foreach (Vector2Int position in pathwayPositions)
        {
            generatedPathway.Add(Instantiate(pathwayTile, new Vector3(position.x, 0, position.y) * sizeMultiplier, Quaternion.identity, pathwayParent));
        }
    }

    public void ClearGeneratedTiles()
    {
#if UNITY_EDITOR
        for (int i = wallParent.childCount; i > 0; --i)
            DestroyImmediate(wallParent.GetChild(0).gameObject);

        for (int i = pathwayParent.childCount; i > 0; --i)
            DestroyImmediate(pathwayParent.GetChild(0).gameObject);

        for (int i = floorParent.childCount; i > 0; --i)
            DestroyImmediate(floorParent.GetChild(0).gameObject);
#else
        for (int i = wallParent.childCount; i > 0; --i)
            Destroy(wallParent.GetChild(0).gameObject);

        for (int i = pathwayParent.childCount; i > 0; --i)
            Destroy(pathwayParent.GetChild(0).gameObject);

        for (int i = floorParent.childCount; i > 0; --i)
            Destroy(floorParent.GetChild(0).gameObject);
#endif

        basicWalls.Clear();
        generatedSpecialWalls.Clear();
        generatedPathway.Clear();
        generatedFloors.Clear();

    }
}
