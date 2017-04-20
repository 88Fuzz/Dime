using UnityEngine;

/*
 * Treated as an "end action" for when all Enemies have been killed in a room.
 * Any actions that should be triggered once a room is cleared out should implement RoomClearAction
 */
public abstract class RoomClearAction : ScriptableObject 
{
    /*
     * Called when every enemy in the room has been killed.
     */
    public abstract void OnRoomClear(Room room);
}