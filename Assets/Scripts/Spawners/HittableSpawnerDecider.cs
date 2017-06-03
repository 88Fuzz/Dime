using UnityEngine;

/*
 * Returns the set of enemies that should be spawned in the room.
 */
[CreateAssetMenu(fileName = "HittableSpawnerDecider", menuName = "ScriptableObjects/Misc/HittableSpawnerDecider")]
public class HittableSpawnerDecider : ScriptableObject
{
    public int numberOfEnemies;
    public Hittable[] enemies;

    /*
     * Returns the set of enemies that should be spawned in the room.
     */
    public Hittable[] GetHittablesForRoom()
    {
        //TODO constantly creating arrays here is possibly expensive? Maybe have a set of arrays that are free that you can use?
        //TODO object pooling
        Hittable[] hittables = new Hittable[numberOfEnemies];
        for (int i = 0; i < numberOfEnemies; i++)
            hittables[i] = enemies[LevelRandomNumberGenerator.levelRNG.GetValueInRange(0, enemies.Length)];

        return hittables;
    }
}