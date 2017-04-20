using UnityEngine;
/*
 * Takes a Room that has been cleared and makes sure the next room the player should enter will be active.
 */
[CreateAssetMenu(fileName = "ActivateNextRoom", menuName = "ScriptableObjects/Rooms/RoomClearAction/ActivateNextRoom")]
public class ActivateNextRoom : RoomClearAction
{
    public override void OnRoomClear(Room room)
    {
        room.nextRoom.RoomSpawned();
        room.nextRoom.gameObject.SetActive(true);
    }
}