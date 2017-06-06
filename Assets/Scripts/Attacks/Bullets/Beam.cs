using UnityEngine;

public class Beam : MyMonoBehaviour
{
    private static readonly Vector3 HALF_SIZE = new Vector3(.2f,.2f,.1f);
    private static readonly float MAX_DISTANCE = 100f;

    public LayerMask layerMask;

    private LineRenderer lineRenderer;
    private Vector3[] linePositions;

    protected override void MyAwake()
    {
        //TODO object pooling
        linePositions = new Vector3[2];
        lineRenderer = GetComponent<LineRenderer>();
    }

    protected override void MyFixedUpdateWithDeltaTime(float myDeltaTime, float timeScale)
    {
        //this needs to be tested, however it may be more fun to create the time bending thing
        RaycastHit hitInfo;
        if(!Physics.BoxCast(transform.position, HALF_SIZE, transform.forward, out hitInfo, transform.rotation, MAX_DISTANCE, layerMask, QueryTriggerInteraction.Ignore))
        {
            Debug.Log("NOTHING WAS FOUND! There should always be something found");
            return;
        }

        linePositions[0].Set(transform.position.x, transform.position.y, transform.position.z);
        linePositions[1].Set(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
    }
}