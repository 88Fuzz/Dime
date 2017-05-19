using UnityEngine;

/*
 * Class that listens to when the player's SpecialAction is fully charged.
 */
public abstract class SpecialFullListener : ScriptableObject
{
    /*
     * Called once the SpecialAction is available for the player to use.
     */
    public abstract void SpecialBarFull(SpecialManager specialManager);
}