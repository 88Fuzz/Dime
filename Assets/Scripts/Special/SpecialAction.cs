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
    public abstract void SpecialRemoved(SpecialManager specialManager);

    /*
     * Called when the Special is being equiped. Any work that needs to be done to set up the special action
     * should be done here.
     */
    public abstract void SpecialEquiped(SpecialManager specialManager);
}