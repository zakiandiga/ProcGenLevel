using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathwayGen : AbstractMapGen
{
    [Tooltip("Length of each pathway to create")]
    [SerializeField] private int pathwayLength = 14;

    [Tooltip("Number of pathway to create")]
    [SerializeField] private int pathwayCount = 5;

    [Tooltip("Percentage of pathway space occupied by the room to be created")]
    [SerializeField] [Range(0.1f, 1f)] private float roomPercentage = 0.7f;

    //[SerializeField] private RandomWalkGen randomWalkGen;

    //[SerializeField] private List<Vector3> roomsCenter = new List<Vector3>();

    //roomsDictionary: Key = roomCenter, Value = roomFloorPositions
    private Dictionary<Vector2Int, HashSet<Vector2Int>> roomsDictionary = new Dictionary<Vector2Int, HashSet<Vector2Int>>();

    //private HashSet<Vector2Int> roomFloors = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> pathwayFloors = new HashSet<Vector2Int>();

    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> pathwayPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();

        CreatePathway(pathwayPositions, potentialRoomPositions);

        HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);

        List<Vector2Int> deadEnds = IdentifyDeadEnds(pathwayPositions);

        CreateRoomsAtDeadEnd(deadEnds, roomPositions);

        //Debug draw room center
        foreach (var roomPos in potentialRoomPositions)
        {
            //roomsCenter.Add(new Vector3(roomPos.x, 1, roomPos.y) * floorVisualizer.SizeMultiplier);
        }

        //pathwayPositions.UnionWith(roomPositions);
        pathwayPositions.ExceptWith(roomPositions);
        pathwayFloors = pathwayPositions;

        floorVisualizer.PaintPathways(pathwayFloors);

        floorVisualizer.PaintFloor(roomPositions);

        pathwayPositions.UnionWith(roomPositions);
        
        wallGenerator.CreateWall(pathwayPositions, floorVisualizer);
    }

    private List<Vector2Int> IdentifyDeadEnds(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();

        foreach(var position in floorPositions)
        {
            int adjecentCount = 0;
            foreach(var direction in Direction2D.basicDirectionList)
            {
                if (floorPositions.Contains(position + direction))
                    adjecentCount++;
            }

            if (adjecentCount == 1)
                deadEnds.Add(position);
        }

        return deadEnds;
    }

    private void CreatePathway(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
    {
        Vector2Int currentPosition = startPos;
        potentialRoomPositions.Add(currentPosition);


        for (int i = 0; i < pathwayCount; i++)
        {
            var pathway = ProcGen.RandomPathway(currentPosition, pathwayLength);
            currentPosition = pathway[pathway.Count - 1];
            potentialRoomPositions.Add(currentPosition);
            floorPositions.UnionWith(pathway);
        }

        //pathwayFloors = floorPositions;
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercentage);

        //randomly sort the potentialRoomPositions, put it to the roomToCreate list
        List<Vector2Int> roomsToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();

        foreach (Vector2Int roomCenter in roomsToCreate)
        {
            HashSet<Vector2Int> roomFloors = RunRandomWalk(roomCenter);
            AddToRoomDictionary(roomCenter, roomFloors);
            roomPositions.UnionWith(roomFloors);
        }

        return roomPositions;
    }

    private void AddToRoomDictionary(Vector2Int roomCenter, HashSet<Vector2Int> roomFloors)
    {
        if(!roomsDictionary.ContainsKey(roomCenter))
            roomsDictionary.Add(roomCenter, roomFloors);
    }

    private void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
    {
        foreach (var position in deadEnds)
        {
            if(!roomFloors.Contains(position))
            {
                var room = RunRandomWalk(position);

                roomFloors.UnionWith(room);
            }
        }
    }

    private void OnDrawGizmos()
    {

    }
}
