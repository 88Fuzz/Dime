using UnityEngine;
using System.Collections.Generic;
using System;

/*
 * As you can probably guess my the class name, the event manager sends messages between components.
 * TODO you should explore this being a scriptable object. Actually, everything that's being used as a singleton might benifit from
 * being a sriptable object?
 *
 *
 * TODO I would love for this to be expanded so that parameters can be passed around. But it's late and I need to go to bed.
 * So figure that out some time. One way to do it is have some base Parameter class that gets extended for all the event types.
 */
public class EventManager : MonoBehaviour
{
    public enum EventName
    {
        HittableSpawned,
        HittableDestroyed
    }
    public delegate void EventListener(EventName eventName);

    private Dictionary<EventName, List<EventListener>> eventListeners;

    public void Awake()
    {
        eventListeners = new Dictionary<EventName, List<EventListener>>();
        foreach(EventName eventName in Enum.GetValues(typeof(EventName)))
        {
            eventListeners.Add(eventName, new List<EventListener>(1));
        }
    }

    public void RegisterListener(EventName eventName, EventListener listener)
    {
        Debug.Log("Requesting to be a listener to event: " + eventName + " and listener " + listener);
        List<EventListener> listeners = eventListeners[eventName];
        listeners.Add(listener);
    }

    public void DeregisterListener(EventName eventName, EventListener listener)
    {
        Debug.Log("Requesting to no longer be a listener to event: " + eventName + " and listener " + listener);
        List<EventListener> listeners = eventListeners[eventName];
        listeners.RemoveAll(item => (item == listener));
    }

    public void PublishEvent(EventName eventName)
    {
        List<EventListener> listeners = eventListeners[eventName];
        Debug.Log("Event that has been published: " + eventName + " number of listeners: " + listeners.Count);
        foreach(EventListener listener in listeners)
        {
            listener(eventName);
        }
    }
}