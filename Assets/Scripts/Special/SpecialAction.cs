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

    /*
     * Called when the Special is being un-equiped. Any work that was done when the special was equiped
     * should be undone here.
     */
    //TODO figure out how to handle this? Where should it live? Should the pickup own it? What about setting up the Special?
    //public abstract void SpecialRemoved(SpecialManager specialManager);
}