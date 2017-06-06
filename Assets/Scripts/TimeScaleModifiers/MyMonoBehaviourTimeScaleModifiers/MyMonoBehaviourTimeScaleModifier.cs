using UnityEngine;

/*
 * Modifies the timeScale based on the MyMonoBehavior
 */
public abstract class MyMonoBehaviourTimeScaleModifier : ScriptableObject
{
    /*
     * Modifies the timeScale based on the MyMonoBehavior
     */
    public abstract float ModifyTimeScale(MyMonoBehaviour entity, float timeScale);
}