using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ProcGen
{
    //random walk
    public static HashSet<Vector2Int> RandomWalk(Vector2Int startPos, int length)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();

        path.Add(startPos);
        var prevPos = startPos;

        for(int i = 0; i < length; i++)
        {
            var newPos = prevPos + (Direction2D.GetRandomDirection());
            path.Add(newPos);
            prevPos = newPos;
        }

        return path;
    }

    public static List<Vector2Int> RandomPathway(Vector2Int startPosition, int pathwayLength)
    {
        List<Vector2Int> pathways = new List<Vector2Int>();
        var direction = Direction2D.GetRandomDirection();
        var currentPosition = startPosition;
        pathways.Add(currentPosition);

        for (int i = 0; i < pathwayLength; i++)
        {
            currentPosition += direction;
            pathways.Add(currentPosition);
        }

        return pathways;

    }

    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight)
    {        
        Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
        List<BoundsInt> roomsList = new List<BoundsInt>();

        roomsQueue.Enqueue(spaceToSplit);

        while(roomsQueue.Count > 0)
        {
            var room = roomsQueue.Dequeue();
            if(room.size.y >= minHeight && room.size.x >= minWidth)
            {
                if(Random.value < 0.5f) //randomize between horizontal and vertical split priority
                {  
                    if(room.size.y >= minHeight*2)
                    {                        
                        SplitHorizontally(minHeight, roomsQueue, room);
                    }

                    else if(room.size.x >= minWidth*2)
                    {
                        SplitVertically(minWidth, roomsQueue, room);
                    }

                    else if(room.size.x >= minWidth && room.size.y >= minHeight)
                    {
                        roomsList.Add(room);
                    }
                }
                else
                {
                    if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth, roomsQueue, room);
                    }

                    else if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minHeight, roomsQueue, room);
                    }

                    else if (room.size.x >= minWidth && room.size.y >= minHeight)
                    {
                        roomsList.Add(room);
                    }
                }
            }
        }

        return roomsList;
    }

    private static void SplitVertically(int minWidth, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        var xSplit = Random.Range(1, room.size.x);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(xSplit, room.size.y, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z), 
                                        new Vector3Int(room.size.x - xSplit, room.size.y, room.size.z));

        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }

    private static void SplitHorizontally(int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        var ySplit = Random.Range(1, room.size.y);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(room.size.x, ySplit, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y + ySplit, room.min.z), 
                                        new Vector3Int(room.size.x, room.size.y - ySplit, room.size.z));

        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }
}

public static class Direction2D
{
    public static List<Vector2Int> basicDirectionList = new List<Vector2Int>
    {
        new Vector2Int(0,1),
        new Vector2Int(1,0),
        new Vector2Int(0,-1),
        new Vector2Int(-1,0),
    };

    public static List<Vector2Int> diagonalDirectionList = new List<Vector2Int>
    {
        new Vector2Int(1,1),
        new Vector2Int(1,-1),
        new Vector2Int(-1,-1),
        new Vector2Int(-1,1),
    };

    public static List<Vector2Int> eightDirectionalList = new List<Vector2Int>
    {
        new Vector2Int(0,1), //N
        new Vector2Int(1,1), //NE
        new Vector2Int(1,0), //E
        new Vector2Int(1,-1), //SE
        new Vector2Int(0,-1), //S
        new Vector2Int(-1,-1), //SW
        new Vector2Int(-1,0), //W
        new Vector2Int(-1,1), //NW
    };

    public static Vector2Int GetRandomDirection()
    {
        return basicDirectionList[Random.Range(0,basicDirectionList.Count)];
    }
}