using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5;
    public Vector3 offset = new Vector3(0, 30, -15);

    public void FixedUpdate()
    {
        Vector3 targetCameraPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCameraPosition, smoothing * Time.deltaTime);
        //TODO I do not like this look at, but it seems to be necessary. Figure out how to fix it!
        transform.LookAt(target);
    }
}