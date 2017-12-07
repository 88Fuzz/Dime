using System.Collections.Generic;
using UnityEngine;

/*
 * Takes all the Enemies that correspond to a room and spawns them in the room.
 */
[CreateAssetMenu(fileName = "SpawnEnemiesInRoom", menuName = "ScriptableObjects/Rooms/RoomStartAction/SpawnEnemiesInRoom")]
public class SpawnEnemiesInRoom : RoomStartAction
{
    /*
     * Takes all the Enemies that correspond to a room and spawns them in the room.
     */
    public override void OnPlayerEnter(Room room)
    {
        //If the room does not have any spawners, it doesn't have a place to put the enemies. So just ignore the enemies.
        //TODO there's probably no point in having enemies in a room if it's impossible to spawn them. Probably fix this.
        if (room.spawners.Length == 0)
            return;

        int i = 0;
        foreach (Hittable enemy in room.enemies)
        {
            //If the spawner is not active, move to the next and try to spawn the enemy.
            while(!room.spawners[i].Spawn(enemy, room))
            {
                if (++i >= room.spawners.Length)
                    i = 0;
            }
            if (++i >= room.spawners.Length)
                i = 0;
        }
    }
}