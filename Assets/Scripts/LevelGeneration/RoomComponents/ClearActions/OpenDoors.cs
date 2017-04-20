using UnityEngine;

[CreateAssetMenu(fileName = "OpenDoorsOnClear", menuName = "ScriptableObjects/Rooms/RoomClearAction/OpenDoorsOnClear")]
public class OpenDoors : RoomClearAction
{
    public override void OnRoomClear(Room room)
    {
        foreach(Door door in room.doors)
        {
            if (door.isConnected)
                door.Open();
        }
    }
}