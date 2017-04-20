using UnityEngine;

[CreateAssetMenu(fileName = "GenerateRoomOnEnd", menuName = "ScriptableObjects/Rooms/RoomClearAction/GenerateRoomOnEnd")]
public class GenerateRoomOnEnd : RoomClearAction
{
    public override void OnRoomClear(Room room)
    {
        FloorGenerator floorGenerator = Singleton<FloorGenerator>.Instance;
        floorGenerator.GenerateRoomOnEnd();
    }
}