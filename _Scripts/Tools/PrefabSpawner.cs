using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PrefabSpawner : MonoBehaviour
{

    private int numberOfRooms => roomGenerator.RoomsDictionary.Count;
    
    public GameObject playerSpawner;
    public GameObject exitDoor;

    public RoomGenerator roomGenerator;

    private Dictionary<Vector2Int, RoomType> roomTypeData = new Dictionary<Vector2Int, RoomType>();

    private Queue<Vector2Int> roomsQueue = new Queue<Vector2Int>();
    //Get the roomDictionary
    //Sort and tag all the keys

    //Get the room with PlayerSpawner tag

    //get a good position for the prefabToSpawn to be placed

    //!!!!define "good position" for each prefab

    //put player spawner prefab on that room

    private void InitiatingPrefabPlacement(WallGenerator w)
    {
        foreach (Vector2Int room in roomGenerator.RoomsDictionary.Keys)
        {
            roomsQueue.Enqueue(room);
        }
        
        AssignStartRoom();

        //Count the number of rooms
        //There has to be at least 2 rooms to proceed, else return back to RoomGenerator
        //assign roomType to each room centers
        //First 2 room to assign is StartRoom and ExitRoom
        //1 StartRoom, 1 ExitRoom, 
    }

    public void AssignStartRoom()
    {
        AssignStartRoom(RoomType.StartRoom);

        Debug.Log("Start room assigned");

        AssignExitRoom(RoomType.ExitRoom);

        Debug.Log("Exit room assigned");
    }


    public void ClearSpawnedPrefab()
    {
        roomTypeData.Clear();
    }

    public void AssignStartRoom(RoomType roomType)
    {
        Vector2Int roomToAssign = roomsQueue.Peek();        
        roomTypeData.Add(roomToAssign, roomType);
        Debug.Log("room " + roomToAssign + " is a " + roomTypeData[roomToAssign]);
        Instantiate(playerSpawner, new Vector3(roomToAssign.x, 1, roomToAssign.y), Quaternion.identity);

        roomsQueue.Dequeue();
    }
    
    private void AssignExitRoom(RoomType exitRoom)
    {
        Vector2Int roomToAssign = roomsQueue.Peek();
        roomTypeData.Add(roomToAssign, exitRoom);
        Debug.Log("room " + roomToAssign + " is a " + roomTypeData[roomToAssign]);

        Instantiate(exitDoor, new Vector3(roomToAssign.x, 1, roomToAssign.y), Quaternion.identity);

        roomsQueue.Dequeue();
    }

    public Vector2Int FindSpawnLocation()
    {
        Vector2Int locationToSpawn = Vector2Int.zero;

        return locationToSpawn;
    }

    private void OnEnable()
    {
        WallGenerator.OnMapCreationFinished += InitiatingPrefabPlacement;
    }

    private void OnDisable()
    {
        WallGenerator.OnMapCreationFinished -= InitiatingPrefabPlacement;
    }
}

public enum RoomType
{
    StartRoom,
    ExitRoom,
    EnemyRoom,
    BossRoom,
    TreasureRoom,
    MiningRoom,
    SpecialRoom
}

