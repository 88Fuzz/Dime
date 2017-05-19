using UnityEngine;

/*
 * Class called when the SpecialAction is used.
 */
public abstract class SpecialUsedListener : ScriptableObject
{
    /*
     * Player has activated Special
     */
    public abstract void SpecialUsed(SpecialManager specialManager);
}