using System.Collections.Generic;
using UnityEngine;

/*
 * Manages when and how the MyMonoBehaviors will have the MyFixedUpdate called.
 */
public class MyMonoBehaviourManager : MonoBehaviour
{
    private List<TimeScaleModifier> globalTimeScaleModifiers;
    private List<MyMonoBehaviorTimeScaleModifier> myMoboBehaviorTimeScaleModifiers;
    private float timeScale;
    private MyMonoBehaviour first;
    private MyMonoBehaviour last;
    private float blah;

    public void Awake()
    {
        globalTimeScaleModifiers = new List<TimeScaleModifier>(4);
        myMoboBehaviorTimeScaleModifiers = new List<MyMonoBehaviorTimeScaleModifier>(4);
        //TODO remove this temp code please
        ActionManager manager = Singleton<ActionManager>.Instance;
        manager.RegisterStartButtonListener(InputButton.JUMP, TempDown);
        manager.RegisterEndButtonListener(InputButton.JUMP, TempUp);
        first = null;
        last = null;
        blah = 1;
    }

    private void TempDown(InputButton inputButton)
    {
        blah = .5f;
    }

    private void TempUp(InputButton inputButton)
    {
        blah = 1f;
    }

    public void FixedUpdate()
    {
        float timeScale = GetTimeScale();
        MyMonoBehaviour current = first;
        while(current != null)
        {
            float currentTimeScale = GetMyMonoBehaviorBasedTimeScale(current, timeScale);
            //Dont forget to remove the random lines you have commented out
            //current.MyFixedUpdate(currentTimeScale);
            current.MyFixedUpdate(blah);
            current = current.Next;
        }
    }

    /*
     * Adds the MyMonoBehavior entity to the list of entities to update each FixedUpdate
     */
    public void RegisterMyMonoBehavior(MyMonoBehaviour entity)
    {
        //Actually, It looks like this is the place with the bug?
        if(first == null)
        {
            first = entity;
            last = entity;
            entity.Next = null;
            entity.Previous = null;
            return;
        }

        entity.Previous = last;
        entity.Next = null;
        last.Next = entity;
        last = entity;
    }

    /*
     * Removes the MyMonoBehavior entity to the list of entities to update each FixedUpdate
     */
    public void DeregisterMyMonoBehavior(MyMonoBehaviour entity)
    {
        if(entity == first && entity == last)
        {
            first = null;
            last = null;
            ClearLinks(entity);
            return;
        }

        if (entity == first)
            first = entity.Next;
        if (entity == last)
            last = entity.Previous;

        if (entity.Previous != null)
            entity.Previous.Next = entity.Next;
        if (entity.Next != null)
            entity.Next.Previous = entity.Previous;

        ClearLinks(entity);
    }

    private string GetName(GameObject gobject)
    {
        return gobject.name;
    }

    private void ClearLinks(MyMonoBehaviour entity)
    {
        entity.Next = null;
        entity.Previous = null;
    }

    private float GetTimeScale()
    {
        float timeScale = Time.deltaTime;
        foreach (TimeScaleModifier timeScaleModifier in globalTimeScaleModifiers)
            timeScale = timeScaleModifier.ModifyTimeScale(timeScale);

        return timeScale;
    }

    private float GetMyMonoBehaviorBasedTimeScale(MyMonoBehaviour entity, float timeScale)
    {
        foreach (MyMonoBehaviorTimeScaleModifier timeScaleModifier in myMoboBehaviorTimeScaleModifiers)
            timeScale = timeScaleModifier.ModifyTimeScale(entity, timeScale);

        return timeScale;
    }
}