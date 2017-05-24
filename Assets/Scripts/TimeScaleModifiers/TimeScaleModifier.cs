using UnityEngine;

/*
 * Modify the timescale for the component.
 */
public abstract class TimeScaleModifier: ScriptableObject
{
    /*
     * Modify the timescale for the component.
     */
    public abstract float ModifyTimeScale(float timeScale);
}