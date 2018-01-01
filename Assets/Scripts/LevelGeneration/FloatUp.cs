using UnityEngine;

public class FloatUp : MonoBehaviour
{
    public float targetPosition;
    public float movePercent;
    public float minimumPositionTolerance;
    public EventManager.EventName eventName;

    private bool finished;

    private void Awake()
    {
        finished = false;
    }

    public void FixedUpdate()
    {
        // Debug.Log("Name: " + gameObject.name + " has a targetPosition of: " + targetPosition + " ID: " + gameObject.GetInstanceID());
        if(finished)
            return;

        SetYPosition(transform.position.y + (targetPosition - transform.position.y) * movePercent * Time.deltaTime);

        if(targetPosition - transform.position.y < minimumPositionTolerance)
        {
            SetYPosition(targetPosition);
            Singleton<EventManager>.Instance.PublishEvent(eventName);
            finished = true;
        }
    }

    private void SetYPosition(float y)
    {
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}