using System.Collections.Generic;
using UnityEngine;

/*
 * This code is based off of https://github.com/DMeville/Unity3d-Dungeon-Generator
 * TODO add more stuff here
 *
 * I don't think FloorGenerator needs to be MonoBehaviour. At most is should be a ScriptableObject.
 * The long term idea of this is to have it create a background thread to create these rooms. At that point
 * the FloorGeneratorCreator should have all the FloorGenerator information, like randomRoomTryCount
 * and randomUnconnectedDoorTryCount and intialRoomCount and floorInformation etc.
 */
public class FloorGenerator : MonoBehaviour
{
    /*
     * Holder struct to return the doors used to connect two rooms
     */
    private struct ConnectedDoors
    {
        public Door sourceDoor;
        public Door destinationDoor;
    };

    public FloorInformation floorInformation;
    public int initialRoomCount = 2;
    public int randomUnconnectedDoorTryCount = 20;
    public int randomRoomTryCount = 20;

    private Grid grid;
    private Room lastRoom;

    public void Awake()
    {
        StartGeneration();
    }

    private void StartGeneration()
    {
        grid = new Grid();

        GameObject spawnRoomGameObject = Instantiate(floorInformation.GetRandomSpawnRoom().gameObject, Vector3.zero, Quaternion.identity);
        spawnRoomGameObject.name = "SpawnRoom";
        //startRoom = roomGameObject;
        spawnRoomGameObject.transform.parent = gameObject.transform;
        Room spawnRoom = spawnRoomGameObject.GetComponent<Room>();
        GridGenerator gridGenerator = spawnRoomGameObject.GetComponent<GridGenerator>();
        gridGenerator.RecalculateBounds();
        AddAllGridCells(gridGenerator.gridCells);

        //TODO this may be a really stupid way of iterating
        Room previousRoom = spawnRoom;
        int roomCount = 0;
        while (roomCount < initialRoomCount)
        {
            Room generatedRoom = GenerateNeighborRooms(previousRoom);
            //Will this cause errors if we continue anyway? yeah probably.
            if (generatedRoom == null)
                continue;

            generatedRoom.gameObject.SetActive(false);
            previousRoom.nextRoom = generatedRoom;
            previousRoom = generatedRoom;
            roomCount++;
        }
        lastRoom = previousRoom;

        //process doors
        /*for (int i = 0; i < rooms.Count; i++)
        {
            for (int j = 0; j < rooms[i].doors.Count; j++)
            {
                if (rooms[i].doors[j].door == null)
                {
                    Door d = ((GameObject)Instantiate(data.sets[dungeonSet].doors[0].gameObject)).GetComponent<Door>();
                    doors.Add(d);
                    rooms[i].doors[j].door = d;
                    rooms[i].doors[j].sharedDoor.door = d;
                    //
                    d.gameObject.transform.position = rooms[i].doors[j].transform.position;
                    d.gameObject.transform.rotation = rooms[i].doors[j].transform.rotation;
                    d.gameObject.transform.parent = this.gameObject.transform;
                }
            }
        }*/
        //locked doors and keys, etc come next. 

        spawnRoom.LevelGenerationDone(floorInformation.unusedDoor);
        GenerationComplete(spawnRoom);
        //Debug.Log("DungeonGenerator::Generation completed : " + DDebugTimer.Lap() + "ms");
    }

    public Room GenerateNeighborRooms(Room processRoom) {
        Door[] doors = processRoom.doors;
        if(processRoom == null || doors.Length <= 1)
        {
            //Nothing to do, the room only has a single entry point or has no doors. This is a bug and should never happen.
            return null;
        }

        /*
         * Create a mutable list of all possible rooms.
         * TODO In the future we should pull the room from this list, because if there is a conflict we can remove it from this list.
         */
        List<Room> roomOptions = new List<Room>(floorInformation.rooms);
        GameObject possibleRoomObject;

        ConnectedDoors newDoors;
        int roomTryCount = 0;
        do
        {
            //TODO, use the roomOptions instead of getting a random room from the floor information.
            possibleRoomObject = Instantiate(floorInformation.GetRandomRoom().gameObject);
            possibleRoomObject.transform.parent = gameObject.transform;
            Room possibleRoom = possibleRoomObject.GetComponent<Room>();

            newDoors = ConnectRooms(processRoom, possibleRoom);
            if(newDoors.destinationDoor == null || newDoors.sourceDoor == null)
            {
                //It was impossible to connect the room, TODO probably do something else in this situation.
                //TODO, maybe do this stuff: return for now. In the future you should add rooms with only one door.
                //OR return and do nothing may be the best option
                DestroyImmediate(possibleRoomObject);
                possibleRoomObject = null;
                continue;
            }

            //Test for overlap
            GridGenerator possibleGridGenerator = possibleRoomObject.GetComponent<GridGenerator>();
            foreach(GameObject cell in possibleGridGenerator.gridCells)
            {
                if(grid.IsGridPositionTaken(cell.transform.position))
                {
                    DestroyImmediate(possibleRoomObject);
                    possibleRoomObject = null;
                    //TODO Remove the room from the roomOptionsList!
                    break;
                }
            }
            //TODO add more checks here. One example would be if two expected door positions overlap.
        } while (possibleRoomObject == null && ++roomTryCount < randomRoomTryCount);

        AddAllGridCells(possibleRoomObject.GetComponent<GridGenerator>().gridCells);
        Room newRoom = possibleRoomObject.GetComponent<Room>();
        newDoors.sourceDoor.ConnectDoor(floorInformation.closedDoor, newDoors.destinationDoor, false);
        newDoors.destinationDoor.ConnectDoor(floorInformation.closedDoor, newDoors.sourceDoor, true);
        newRoom.LevelGenerationDone(floorInformation.unusedDoor);

        return newRoom;
    }

    public void GenerateRoomOnEnd()
    {
        int tryCount = 0;
        Room newRoom = null;
        do
        {
            newRoom = GenerateNeighborRooms(lastRoom);
        } while (++tryCount < randomRoomTryCount && newRoom == null);

        lastRoom.nextRoom = newRoom;
        newRoom.nextRoom = null;
        newRoom.gameObject.SetActive(false);
        lastRoom = newRoom;
    }

    private void AddAllGridCells(GameObject[] gridCells)
    {
        foreach(GameObject cell in gridCells)
        {
            grid.AddGridPosition(cell.transform.position);
        }
    }

    private ConnectedDoors ConnectRooms(Room sourceRoom, Room destinationRoom)
    {
        ConnectedDoors connectedDoors = new ConnectedDoors();
        Door sourceDoor = getDoor(sourceRoom);
        Door destinationDoor = getDoor(destinationRoom);
        if (sourceDoor == null || destinationDoor == null)
            return connectedDoors;

        connectedDoors.sourceDoor = sourceDoor;
        connectedDoors.destinationDoor = destinationDoor;

        destinationRoom.transform.rotation = Quaternion.AngleAxis((sourceDoor.transform.eulerAngles.y - destinationDoor.transform.eulerAngles.y) + 180f, Vector3.up);
        Vector3 translate = sourceDoor.transform.position - destinationDoor.transform.position;
        destinationRoom.transform.position += translate;
        destinationRoom.GetComponent<GridGenerator>().RecalculateBounds();

        return connectedDoors;
    }

    private Door getDoor(Room room)
    {
        Door door = room.GetRandomUnconnectedDoor(randomUnconnectedDoorTryCount);

        if (door != null)
            return door;

        return room.GetFirstUnconnectedDoor();
    }

    private void GenerationComplete(Room spawnRoom)
    {
        spawnRoom.RoomActivated();
        /*Room currentRoom = spawnRoom;
        while(currentRoom != null)
        {
            currentRoom.RoomSpawned();
            currentRoom = currentRoom.nextRoom;
        }*/
    }
}