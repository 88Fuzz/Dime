using System.Collections.Generic;
using UnityEngine;

/*
 * Any component that can be affected by time manipulation should implement this class.
 */
public abstract class MyMonoBehaviour : MonoBehaviour
{
    protected delegate void FixedUpdateAction(float myDeltaTime, float timeScale);

    public List<TimeScaleModifier> timeScaleModifiers;

    private MyMonoBehaviour previous;
    private MyMonoBehaviourManager manager;
    private MyMonoBehaviour next;
    private bool myEnabled;

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
        manager = Singleton<MyMonoBehaviourManager>.Instance;
        //TODO object pooling
        timeScaleModifiers = new List<TimeScaleModifier>(3);
        manager.RegisterMyMonoBehavior(this);
        myEnabled = true;
        MyAwake();
    }

    public void MyEnable()
    {
        if(myEnabled)
            return;
        manager.RegisterMyMonoBehavior(this);
        gameObject.SetActive(true);
        myEnabled = true;
    }

    public void MyDisable()
    {
        if(!myEnabled)
            return;
        manager.DeregisterMyMonoBehavior(this);
        gameObject.SetActive(false);
        myEnabled = false;
    }

    public bool MyIsEnabled()
    {
        return myEnabled;
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