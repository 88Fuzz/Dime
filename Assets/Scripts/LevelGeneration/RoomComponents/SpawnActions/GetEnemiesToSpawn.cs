using UnityEngine;

/*
 * Assigns the enemies that should be spawned in the room
 */
[CreateAssetMenu(fileName = "GetEnemiesToSpawn", menuName = "ScriptableObjects/Rooms/RoomSpawnAction/GetEnemiesToSpawn")]
public class GetEnemiesToSpawn : RoomSpawnAction
{
    public HittableSpawnerDecider hittableSpawnerDecider;

    /*
     * Assigns the enemies that should be spawned in the room
     */
    public override void OnRoomActivated(Room room)
    {
        room.enemies = hittableSpawnerDecider.GetHittablesForRoom();
    }

    public override void OnRoomGenerationDone(Room room)
    {
        //Do nothing
    }
}