using UnityEngine;

public class MoveWhenPlayerMoves : MyMonoBehaviour
{
    public LayerMask targetMask;
    public int maxScanDistance;
    public float moveSpeed;
    public float velocityDecay;

    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Transform targetTransform;
    private Vector3 previousTargetPosition;

    protected override void MyAwake()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navMeshAgent.speed = 0;
        //TODO pull out this method of finding the player into some common method.
        Collider[] colliderResults = new Collider[1];
        if(Physics.OverlapSphereNonAlloc(transform.position, maxScanDistance, colliderResults, targetMask, QueryTriggerInteraction.Ignore) > 0)
        {
            Collider collider = colliderResults[0];
            targetTransform = collider.gameObject.transform;
        }
        else
        {
            targetTransform = transform;
            Debug.Log("Yo, I can't find a damn target to hit :(");
        }
        previousTargetPosition = targetTransform.transform.position;
    }

    protected override void MyFixedUpdateWithDeltaTime(float myDeltaTime, float timeScale)
    {
        float distanceMoved = Vector3.Distance(targetTransform.transform.position, previousTargetPosition);
        if (distanceMoved <= 0.05f)
        {
            navMeshAgent.speed = 0;
            navMeshAgent.velocity *= 1 - (velocityDecay * timeScale);
        }
        else
        {
            navMeshAgent.speed = moveSpeed * timeScale;
            navMeshAgent.SetDestination(targetTransform.position);
        }

        previousTargetPosition = targetTransform.transform.position;
    }
}