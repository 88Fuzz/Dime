using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    public CommonRoomActions commonRoomActions;
    //Called when the room is spawned into the world
    public RoomSpawnAction[] spawnActions;
    //Called when the player enters a room
    public RoomStartAction[] startActions;
    //Called when the player leaves the room
    public RoomClearAction[] clearActions;
    public Door[] doors;
    public Spawner[] spawners;
    public Hittable[] enemies;
    public Transform spawnPosition = null;
    public Room nextRoom = null;

    private bool roomCleared = false;
    //ActiveRoom means the player is currently in the room.
    private bool activeRoom = false;
    private int numberOfEnemiesSpawned;

    public void Awake()
    {
        numberOfEnemiesSpawned = 0;
    }

    public void FixedUpdate()
    {
        if(!activeRoom)
            return;

        Debug.Log("Number of enemies: " + numberOfEnemiesSpawned);
        if(numberOfEnemiesSpawned == 0)
            DoClearActions();
    }

    /*
     * Called when a level is placed into the world. The room is considered a valid place but is not currently active.
     */
    public void LevelGenerationDone(GameObject unusedDoorObject)
    {
        foreach(Door door in doors)
        {
            if (door.isConnected)
                continue;

            door.ApplyDoorObject(unusedDoorObject);
        }
        foreach (RoomSpawnAction spawnAction in commonRoomActions.spawnActions)
            spawnAction.OnRoomGenerationDone(this);

        foreach (RoomSpawnAction spawnAction in spawnActions)
            spawnAction.OnRoomGenerationDone(this);
    }

    /*
     * Called when the connected room has all enemies killed
     */
    public void RoomActivated()
    {
        foreach (RoomSpawnAction spawnAction in commonRoomActions.spawnActions)
            spawnAction.OnRoomActivated(this);

        foreach (RoomSpawnAction spawnAction in spawnActions)
            spawnAction.OnRoomActivated(this);
    }

    public void DoClearActions()
    {
        if (roomCleared)
            return;

        Singleton<EventManager>.Instance.DeregisterListener(EventManager.EventName.HittableSpawned, EventPublished);
        Singleton<EventManager>.Instance.DeregisterListener(EventManager.EventName.HittableDestroyed, EventPublished);
        roomCleared = true;
        foreach (RoomClearAction clearAction in commonRoomActions.clearActions)
            clearAction.OnRoomClear(this);
        foreach (RoomClearAction clearAction in clearActions)
            clearAction.OnRoomClear(this);
    }

    public Door GetRandomUnconnectedDoor(int tries)
    {
        for(int i = 0; i < tries; i++)
        {
            Door door = doors[LevelRandomNumberGenerator.levelRNG.GetValueInRange(0, doors.Length)];
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

    public void EventPublished(EventManager.EventName eventName)
    {
        //The way events are triggered and when a room becomes active seems to be fucking up?
            //I think there is and order of operations that is making things break

            //This did not fix anything. There is still broken spawning. Try debugging and see where stuff is being spawned.
            //Also it looks the order in which common room actions take place are variable?
        Debug.Log("Received publish event! " + eventName);
        switch(eventName)
        {
            case EventManager.EventName.HittableSpawned:
                numberOfEnemiesSpawned++;
                break;
            case EventManager.EventName.HittableDestroyed:
                numberOfEnemiesSpawned--;
                break;
            default:
                //Do nothing.
                break;
        }
    }

    //TODO does this need to have an OnTriggerExit to deactivate the room????? Or will that be handled by the room turning "grey" once the player leaves
    public void OnTriggerEnter(Collider other)
    {
        Singleton<EventManager>.Instance.RegisterListener(EventManager.EventName.HittableSpawned, EventPublished);
        Singleton<EventManager>.Instance.RegisterListener(EventManager.EventName.HittableDestroyed, EventPublished);
        //TODO check if the other is the player!
        activeRoom = true;
        foreach (RoomStartAction startAction in commonRoomActions.playerEnterActions)
        {
            startAction.OnPlayerEnter(this);
            Debug.Log("Here it is: " + startAction);
        }
        foreach(RoomStartAction startAction in startActions)
            startAction.OnPlayerEnter(this);
    }
}