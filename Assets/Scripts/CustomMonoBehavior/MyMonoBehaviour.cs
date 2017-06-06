using System.Collections.Generic;
using UnityEngine;

/*
 * Any component that can be affected by time manipulation should implement this class.
 * //TODO figure out how to handle enabling and disabling objects
 * // It will probably be: on enable, set the MonoBehavior to be enabled, add it to the MyMonoBehaviorManager
 * //On disable, set the MonoBehavior to disabled, remove it from the MyMonoBehaviorManger
 */
public abstract class MyMonoBehaviour : MonoBehaviour
{
    public List<TimeScaleModifier> timeScaleModifiers;

    private MyMonoBehaviour previous;
    private MyMonoBehaviour next;
    private MyMonoBehaviourManager manager;

    public MyMonoBehaviour Previous
    {
        get { return previous; }
        set { previous = value; }
    }

    public MyMonoBehaviour Next
    {
        get { return next; }
        set { next = value; }
    }

    public void Awake()
    {
        previous = null;
        next = null;
        timeScaleModifiers = new List<TimeScaleModifier>(3);
        manager = Singleton<MyMonoBehaviourManager>.Instance;
        manager.RegisterMyMonoBehavior(this);
        MyAwake();
    }

    public void MyFixedUpdate(float timeScale)
    {
        float localTimeScale = timeScale;
        foreach (TimeScaleModifier timeScaleModifier in timeScaleModifiers)
            localTimeScale = timeScaleModifier.ModifyTimeScale(localTimeScale);

        MyFixedUpdateWithDeltaTime(Time.deltaTime * timeScale, timeScale);
    }

    public void MyDestroy()
    {
        //TODO is there a performance hit getting the Instance every time? I wouldn't think so
        manager.DeregisterMyMonoBehavior(this);
        Destroy(gameObject);
    }

    public virtual Vector3 GetPosition()
    {
        return transform.position;
    }

    /*
     * Called to be used as an alternative to Awake. Called at the end of Awake.
     */
    protected abstract void MyAwake();

    /*
     * This should be used instead of FixedUpdate. The input myDeltaTime is the engine's 
     * deltaTime multiplied by the timeScale:
     */
    protected abstract void MyFixedUpdateWithDeltaTime(float myDeltaTime, float timeScale);
}