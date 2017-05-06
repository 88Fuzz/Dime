using UnityEngine;

/*
 * Spider movement has 2 movement stages. 1) Moving to a point or 2) standing still. 
 *
 * 1) If a player is in a sphere of detection, the SpiderMovement will move to the point the player is currently at.
 * Else it will pick a random point in the sphere of detection.
 * 
 * 2) While standing still, it will wait for a random amount of time in a range before deciding a point to move to again.
 */
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class SpiderMovement : MonoBehaviour
{
    public LayerMask targetLayer;
    public Range moveWaitTime;
    //TODO Maybe do this better, if It's impossible to get to the destination. Maybe detect it by some other means instead of waiting.
    public float moveTimeout;
    [Range(0,1)]
    public float anticipationMovePercent;

    private Animator animator;
    private GameObject target;
    private SphereCollider sphereCollider;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private float maxMoveDistance;
    private float timer;
    private float anticipationMoveWaitTime;
    private float waitTime;

    private int prejumpHash = Animator.StringToHash("prejump");
    private int jumpHash = Animator.StringToHash("jump");
    private int postjumpHash = Animator.StringToHash("postjump");

    public void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        sphereCollider = GetComponentInChildren<SphereCollider>();
        maxMoveDistance = sphereCollider.radius;

        timer = 0;
        waitTime = 0;
        anticipationMoveWaitTime = 0;
    }

    public void FixedUpdate()
    {
        timer += Time.deltaTime;
        if(!navMeshAgent.enabled)
        {
            if(timer>waitTime)
            {
                Vector3 targetPosition = GetTargetTransform();
                navMeshAgent.enabled = true;
                navMeshAgent.SetDestination(targetPosition);
                SetMoveFlags();
                timer = 0;
            }
            else if(timer > anticipationMoveWaitTime)
            {
                SetPreMoveFlags();
            }
        }
        else
        {
            if(timer > moveTimeout || IsNavMeshFinished())
            {
                navMeshAgent.enabled = false;
                waitTime = Random.value * moveWaitTime.GetDifference() + moveWaitTime.min;
                anticipationMoveWaitTime = waitTime - waitTime * anticipationMovePercent;
                //TODO These ending movements need to happen waaaaaaay earlier in the jump. It looks a bit weird at the moment.
                SetEndMoveFlags();
                timer = 0;
            }
        }
    }

    private void SetPreMoveFlags()
    {
        animator.SetBool(prejumpHash, true);
        animator.SetBool(postjumpHash, false);
    }

    private void SetMoveFlags()
    {
        animator.SetBool(jumpHash, true);
        animator.SetBool(prejumpHash, false);
    }

    private void SetEndMoveFlags()
    {
        animator.SetBool(postjumpHash, true);
        animator.SetBool(jumpHash, false);
    }

    private bool IsNavMeshFinished()
    {
        return navMeshAgent.remainingDistance != Mathf.Infinity && navMeshAgent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete && navMeshAgent.remainingDistance == 0;
    }

    //TODO: Bug alert! if this is ever multiplayer, if 2 players enter the sphere and the first is selected as the target. Then leaves. The second player will never be considered a target.
    public void OnTriggerEnter(Collider collider)
    {
        if(LayerUtils.CompareLayerWithLayerMask(collider.gameObject.layer, targetLayer))
        {
            target = collider.gameObject;
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject == target)
        {
            target = null;
        }
    }

    private Vector3 GetTargetTransform()
    {
        if (target)
        {
            return target.transform.position;
        }

        return transform.position - new Vector3(GetSinglePosition(), transform.position.y,GetSinglePosition());
    }

    private float GetSinglePosition()
    {
        return (Random.value * 2 * maxMoveDistance) - maxMoveDistance;
    }
}
