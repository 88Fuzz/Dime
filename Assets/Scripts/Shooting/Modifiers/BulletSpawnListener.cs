using UnityEngine;

/*
 * Class to do some kind of action every time a bullet is spawned.
 */
public abstract class BulletSpawnListener : ScriptableObject
{
    /*
     * Called when a bullet is just spawned into the world
     */
    public abstract void OnBulletSpawn(Bullet bullet);
}