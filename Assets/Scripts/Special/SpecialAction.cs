using UnityEngine;

/*
 * Called when the Special is fully charged and the player activates the action.
 */
public abstract class SpecialAction : ScriptableObject
{
    /*
     * Called when the Special is fully charged and the player activates the action.
     */
    public abstract void DoAction(SpecialManager specialManager);
}