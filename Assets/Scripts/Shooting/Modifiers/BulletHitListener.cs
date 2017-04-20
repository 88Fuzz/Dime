using UnityEngine;

/*
 * A collection of methods that are called when a bullet hits an object or an enemy.
 */
public abstract class BulletHitListener : ScriptableObject
{
    /*
     * Called when a bullet hits an enemy. Return true if the bullet should be deleted.
     */
    public abstract bool OnEnemyHit(Bullet bullet, Hittable enemy);

    /*
     * Called when a bullet kills an enemy. Return true if the bullet should be deleted.
     */
    public abstract bool OnEnemyKill(Bullet bullet, Hittable enemy);

    /*
     * Called when a bullet hits a static object like a wall or the floor.
     */
    public abstract void OnObjectHit(Bullet bullet, GameObject obj);
}