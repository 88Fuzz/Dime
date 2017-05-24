using UnityEngine;

/*
 * Modifies the timeScale based on the MyMonoBehavior
 */
public abstract class MyMonoBehaviorTimeScaleModifier : ScriptableObject
{
    /*
     * Modifies the timeScale based on the MyMonoBehavior
     */
    public abstract float ModifyTimeScale(MyMonoBehaviour entity, float timeScale);
}