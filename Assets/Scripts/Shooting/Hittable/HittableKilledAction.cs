using UnityEngine;

/*
 * Called when the Hittable has been killed by a Bullet
 */
public abstract class HittableKilledAction : ScriptableObject
{
    /*
     * Called when the Hittable has been killed by a Bullet
     */
    public abstract void HittableHasBeenKilled(Hittable hittable);
}