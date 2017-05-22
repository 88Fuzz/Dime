using UnityEngine;

/*
 * Called by the player to decide how long to wait before shooting another bullet.
 */
public abstract class ShootDelayModifier : ScriptableObject
{
    /*
     * Called to initialize the ShootDelayModifier
     */
    public abstract void InitModifier();

    /*
     * Called by the player to decide how long to wait before shooting another bullet.
     */
    public abstract float GetShootDelay(ShootingManager shootingManager);
}