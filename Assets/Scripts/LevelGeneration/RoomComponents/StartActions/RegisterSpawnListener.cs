using UnityEngine;

[CreateAssetMenu(fileName = "RegisterSpawnListener", menuName = "ScriptableObjects/Rooms/RoomStartAction/RegisterSpawnListener")]
public class RegisterSpawnListener : RoomStartAction
{
    public override void OnPlayerEnter(Room room)
    {
        Singleton<EventManager>.Instance.RegisterListener(EventManager.EventName.HittableSpawned, room.EventPublished);
        Singleton<EventManager>.Instance.RegisterListener(EventManager.EventName.HittableDestroyed, room.EventPublished);
    }
}