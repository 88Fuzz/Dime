using UnityEngine;

/*
 * Once a room has been rotated during level generation, the spawners are all out of whack.
 * This re-aligns the rooms by rotating the spawners around the y axis rotation.
 */
[CreateAssetMenu(fileName = "AlignSpawners", menuName = "ScriptableObjects/Rooms/RoomSpawnAction/AlignSpawners")]
public class AlignSpawners : RoomSpawnAction
{
    /*
     * Once a room has been rotated during level generation, the spawners are all out of whack.
     * This re-aligns the rooms by rotating the spawners around the y axis rotation.
     */
    public override void OnRoomActivated(Room room)
    {
        foreach(Spawner spawner in room.spawners)
        {
            spawner.CalculateSpawnArea();
        }
    }

    public override void OnRoomGenerationDone(Room room)
    {
        //Do nothing
    }
}