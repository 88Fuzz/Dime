using UnityEngine;

public class Room : MonoBehaviour
{
    //Called when the room is spawned into the world
    public RoomSpawnAction[] spawnActions;
    //Called when the player enters a room
    public RoomStartAction[] startActions;
    //Called when the player leaves the room
    public RoomClearAction[] clearActions;
    public Door[] doors;
    public Transform spawnPosition = null;
    public Room nextRoom = null;

    private bool roomCleared = false;
    private int enemyCount = 0;
    //ActiveRoom means the player is currently in the room.
    private bool activeRoom = false;

    // Update is called once per frame
    public void Update()
    {
        if (!activeRoom)
            return;

        if (enemyCount == 0)
            DoClearActions();
    }

    public void LevelGenerated(GameObject unusedDoorObject)
    {
        foreach(Door door in doors)
        {
            if (door.isConnected)
                continue;

            door.ApplyDoorObject(unusedDoorObject);
        }
        foreach (RoomSpawnAction spawnAction in CommonRoomActions.COMMON_SPAWN_ACTIONS)
            spawnAction.OnLevelGenerationDone(this);

        foreach (RoomSpawnAction spawnAction in spawnActions)
            spawnAction.OnLevelGenerationDone(this);
    }

    public void RoomSpawned()
    {
        foreach (RoomSpawnAction spawnAction in CommonRoomActions.COMMON_SPAWN_ACTIONS)
            spawnAction.OnLevelActivated(this);

        foreach (RoomSpawnAction spawnAction in spawnActions)
            spawnAction.OnLevelActivated(this);
    }

    public void DoClearActions()
    {
        if (roomCleared)
            return;

        roomCleared = true;
        foreach (RoomClearAction clearAction in CommonRoomActions.COMMON_CLEAR_ACTIONS)
            clearAction.OnRoomClear(this);
        foreach (RoomClearAction clearAction in clearActions)
            clearAction.OnRoomClear(this);
    }

    public Door GetRandomUnconnectedDoor(int tries)
    {
        for(int i = 0; i < tries; i++)
        {
            Door door = doors[Random.Range(0, doors.Length)];
            if (!door.isConnected)
                return door;
        }

        return null;
    }

    public Door GetFirstUnconnectedDoor()
    {
        foreach (Door door in doors)
        {
            if (!door.isConnected)
                return door;
        }

        return null;
    }

    public void PlayerLeft()
    {
        activeRoom = false;
    }

    //TODO does this need to have an OnTriggerExit to deactivate the room????? Or will that be handled by the room turning "grey" once the player leaves
    public void OnTriggerEnter(Collider other)
    {
        activeRoom = true;
        foreach (RoomStartAction startAction in CommonRoomActions.COMMON_START_ACTIONS)
            startAction.OnPlayerEnter();
        foreach(RoomStartAction startAction in startActions)
            startAction.OnPlayerEnter();
    }
}