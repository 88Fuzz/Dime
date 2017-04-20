using UnityEngine;

/*
 * Simple decider class to determine the number of bullets to spawn.
 */
public abstract class BulletSpawnNumberDecider: ScriptableObject
{
    /*
     * Return the number of bullets that should spawn.
     */
    public abstract int GetNumberOfBulletsToSpawn();
}