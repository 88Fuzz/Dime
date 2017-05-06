using UnityEngine;

/*
 * Called when the Hittable has been hit by a Bullet
*/
public abstract class HittableHitAction : ScriptableObject
{
    /*
     * Called when the Hittable has been hit by a Bullet
     */
    public abstract void HittableHasBeenHit(Hittable hittable);
}