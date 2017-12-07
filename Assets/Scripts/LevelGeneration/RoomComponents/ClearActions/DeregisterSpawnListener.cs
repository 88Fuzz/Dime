using UnityEngine;

[CreateAssetMenu(fileName = "DeregisterSpawnListener", menuName = "ScriptableObjects/Rooms/RoomClearAction/DeregisterSpawnListener")]
public class DeregisterSpawnListener : RoomClearAction
{
    public override void OnRoomClear(Room room)
    {
        Singleton<EventManager>.Instance.DeregisterListener(EventManager.EventName.HittableDestroyed, room.EventPublished);
        Singleton<EventManager>.Instance.DeregisterListener(EventManager.EventName.HittableSpawned, room.EventPublished);
    }
}