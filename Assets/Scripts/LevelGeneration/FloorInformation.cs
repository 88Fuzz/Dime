using System.Collections.Generic;
using UnityEngine;

/*
 * Holder class of all Level generation information, like Rooms and SpawnRooms and everything else.
 */
[CreateAssetMenu(fileName = "FloorInformation", menuName = "ScriptableObjects/Rooms/FloorInformation")]
public class FloorInformation : ScriptableObject 
{
    public GameObject unusedDoor;
    public GameObject closedDoor;
    public List<Room> rooms;
    public List<Room> spawnRooms;

    public Room GetRandomSpawnRoom()
    {
        int position = LevelRandomNumberGenerator.levelRNG.GetValueInRange(0, spawnRooms.Count);
        return spawnRooms[position];
    }

    public Room GetRandomRoom()
    {
        int position = LevelRandomNumberGenerator.levelRNG.GetValueInRange(0, rooms.Count);
        this shit is fucking up now :(
        Debug.Log("position " + position);
        return rooms[0];
    }
}