using UnityEngine;

/*
 * Any activity that should be triggered once a player enters a room should implement the RoomStartAction.
 */
public abstract class RoomStartAction : ScriptableObject 
{
    /*
     * Method called when a player enters the room.
     */
    public abstract void OnPlayerEnter(Room room);
}