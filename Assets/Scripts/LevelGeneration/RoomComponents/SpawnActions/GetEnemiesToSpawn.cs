using UnityEngine;

[CreateAssetMenu(fileName = "GetEnemiesToSpawn", menuName = "ScriptableObjects/Rooms/RoomSpawnAction/GetEnemiesToSpawn")]
public class GetEnemiesToSpawn : RoomSpawnAction
{
    public HittableSpawnerDecider hittableSpawnerDecider;

    public override void OnRoomActivated(Room room)
    {
        room.enemies = hittableSpawnerDecider.GetHittablesForRoom();
    }

    public override void OnRoomGenerationDone(Room room)
    {
        //TODO probably do some spawn action here for enemies
    }
}