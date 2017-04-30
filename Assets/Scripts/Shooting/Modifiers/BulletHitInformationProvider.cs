using UnityEngine;

/*
 * Provider called whenever a bullet hits an object to get the damage information the bullet should inflict.
 */
public abstract class BulletHitInformationProvider : ScriptableObject
{
    /*
     * Return the damage information the bullet should inflict on an object.
     */
    public abstract float GetHitInformation();
}