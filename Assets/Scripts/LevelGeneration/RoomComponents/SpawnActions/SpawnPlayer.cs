using UnityEngine;

/*
 * Spawn a player at the spawnPosition once level generation is completed.
 */
[CreateAssetMenu(fileName = "SpawnPlayer", menuName = "ScriptableObjects/Rooms/RoomSpawnAction/SpawnPlayer")]
public class SpawnPlayer : RoomSpawnAction
{
    public GameObject player;

    public override void OnLevelActivated(Room room)
    {
        //Do Nothing
    }

    public override void OnLevelGenerationDone(Room room)
    {
        Transform spawnPosition = room.spawnPosition;
        if (spawnPosition == null)
            return;
        GameObject playerObject = Instantiate(player, spawnPosition.position, Quaternion.identity, null);
        playerObject.name = "The Real Slim";
    }
}