using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : AbstractMapGen
{
    [Tooltip("")]
    [SerializeField] private int minRoomWidth = 4;

    [Tooltip("")]
    [SerializeField] private int minRoomLength = 4;

    [Tooltip("Overall width of area to make rooms")]
    [SerializeField] private int mapWidth = 20;

    [Tooltip("Overall length of area to make rooms")]
    [SerializeField] private int mapLength = 20;

    [Range(0, 10)]
    [SerializeField] private int offset = 1;

    [Tooltip("use random walk to generate room?")]
    [SerializeField] private bool randomWalkRooms = false;

    //[SerializeField] private RandomWalkGen randomWalkGen;
    public Dictionary<Vector2Int, HashSet<Vector2Int>> RoomsDictionary { get { return roomsDictionary; } }

    private Dictionary<Vector2Int, HashSet<Vector2Int>> roomsDictionary = new Dictionary<Vector2Int, HashSet<Vector2Int>>();

    protected override void RunProceduralGeneration()
    {
        if (roomsDictionary.Count > 0)
            roomsDictionary.Clear();

        CreateRooms();
    }

    private void CreateRooms()
    {
        var roomsList = ProcGen.BinarySpacePartitioning
            (new BoundsInt((Vector3Int)startPos, new Vector3Int(mapLength, mapWidth, 0)), minRoomLength, minRoomWidth);

        HashSet<Vector2Int> roomFloors = new HashSet<Vector2Int>();

        if (!randomWalkRooms)
            roomFloors = CreateSimpleRooms(roomsList);
        else
            roomFloors = CreateRandomRooms(roomsList);

        List<Vector2Int> roomCenters = new List<Vector2Int>();      

        foreach (Vector2Int roomCenter in roomsDictionary.Keys)
        {
            roomCenters.Add(roomCenter);
        }

        HashSet<Vector2Int> pathwaysFloors = ConnectRooms(roomCenters);

        //Separate pathways floors from rooms floors
        pathwaysFloors.ExceptWith(roomFloors);

        floorVisualizer.PaintFloor(roomFloors);
        floorVisualizer.PaintPathways(pathwaysFloors);

        //Create wall based on created floor & pathway
        roomFloors.UnionWith(pathwaysFloors);
        wallGenerator.CreateWall(roomFloors, floorVisualizer);
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floors = new HashSet<Vector2Int>();

        //loop through each BoundsInt
        foreach (var roomCenter in roomsList)
        {
            //prepare the HashSet for this room roomFloors
            HashSet<Vector2Int> roomFloors = new HashSet<Vector2Int>(); //

            for (int column = offset; column < roomCenter.size.x - offset; column++)
            {
                for (int row = offset; row < roomCenter.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)roomCenter.min + new Vector2Int(column, row);

                    roomFloors.Add(position);
                }
            }

            //Update the dictionary
            roomsDictionary.Add((Vector2Int)Vector3Int.RoundToInt(roomCenter.center), roomFloors);

            //Add the room floors to the floors
            floors.UnionWith(roomsDictionary[(Vector2Int)Vector3Int.RoundToInt(roomCenter.center)]);
        }

        return floors;
    }

    private HashSet<Vector2Int> CreateRandomRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floors = new HashSet<Vector2Int>();

        for (int i = 0; i < roomsList.Count; i++)
        {
            var roomBounds = roomsList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(roomCenter);

            HashSet<Vector2Int> roomFloors = new HashSet<Vector2Int>();

            //consider offset
            foreach (var position in roomFloor)
            {
                if (position.x >= (roomBounds.xMin + offset) && position.x <= (roomBounds.xMax - offset)
                    && position.y >= (roomBounds.yMin + offset) && position.y <= (roomBounds.yMax - offset))
                {
                    //floors.Add(position);
                    roomFloors.Add(position);
                }
            }

            roomsDictionary.Add(roomCenter, roomFloors);
            //add the roomFloors HashSet in this roomCenter key to the floors HashSet
            floors.UnionWith(roomsDictionary[roomCenter]);
        }
        return floors;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> pathways = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[UnityEngine.Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int nearestRoom = FindNearestPoint(currentRoomCenter, roomCenters);
            roomCenters.Remove(nearestRoom);
            HashSet<Vector2Int> newPathway = CreatePathway(currentRoomCenter, nearestRoom);
            currentRoomCenter = nearestRoom;
            pathways.UnionWith(newPathway);
        }

        return pathways;
    }

    private HashSet<Vector2Int> CreatePathway(Vector2Int currentRoomCenter, Vector2Int nearestRoom)
    {
        HashSet<Vector2Int> pathways = new HashSet<Vector2Int>();
        var positionC = currentRoomCenter;
        var positionL = positionC;
        var positionR = positionC;

        pathways.Add(positionC);

        while(positionC.y != nearestRoom.y)
        {
            if (nearestRoom.y > positionC.y)
            {
                positionC += Vector2Int.up;
                //
                positionL = positionC + Vector2Int.left;
                positionR = positionC + Vector2Int.right;

            }    

            else if (nearestRoom.y < positionC.y)
            {
                positionC += Vector2Int.down;
                //
                positionL = positionC + Vector2Int.right;
                positionR = positionC + Vector2Int.left;

            }

            pathways.Add(positionC);

            if(!pathways.Contains(positionL) && !pathways.Contains(positionR))
            {
                pathways.Add(positionL);
                pathways.Add(positionR);
            }
        }

        while(positionC.x != nearestRoom.x)
        {
            if (nearestRoom.x > positionC.x)
            {
                positionC += Vector2Int.right;
                //
                positionL = positionC + Vector2Int.up;
                positionR = positionC + Vector2Int.down;
            }

            else if (nearestRoom.x < positionC.x)
            {
                positionC += Vector2Int.left;
                //
                positionL = positionC + Vector2Int.down;
                positionR = positionC + Vector2Int.up;
            }

            pathways.Add(positionC);

            if (!pathways.Contains(positionL) && !pathways.Contains(positionR))
            {
                pathways.Add(positionL);
                pathways.Add(positionR);
            }
        }

        return pathways;
    }

    private Vector2Int FindNearestPoint(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int nearestRoom = Vector2Int.zero;
        float distance = float.MaxValue;

        foreach(var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if(currentDistance < distance)
            {
                distance = currentDistance;
                nearestRoom = position;
            }
        }

        return nearestRoom;
    }
    

    private void OnDrawGizmos()
    {
        if (roomsDictionary.Count > 0)
        {
            /*
            foreach (Vector2Int floor in roomsDictionary.ElementAt(UnityEngine.Random.Range(0, roomsDictionary.Count)).Value)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(new Vector3(floor.x, 1, floor.y), Vector3.one);
            }
            */

            
            foreach (Vector2Int center in roomsDictionary.Keys)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(new Vector3 (center.x, 3, center.y), Vector3.one);


                
                foreach(Vector2Int floors in roomsDictionary[center])
                {
                    Gizmos.DrawWireCube(new Vector3(floors.x, 1, floors.y), Vector3.one);
                }
                
            }    
            
        }
    }
}
