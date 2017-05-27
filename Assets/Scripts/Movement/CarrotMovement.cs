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
public class CarrotMovement : MyMonoBehaviour
{
    /*
     * Function pointer to modify any animator properties.
     * Returns true when the propert is done and should be replaced with the DoNothingChanger.
     */
    private delegate bool AnimatorPropertyChanger(float deltaTime);

    private static readonly int JUMP_HEIGHT_HASH = Animator.StringToHash("jumpHeight");

    public LayerMask targetLayer;
    public Range moveWaitTime;
    //TODO Maybe do this better, if It's impossible to get to the destination. Maybe detect it by some other means instead of waiting.
    public float moveTimeout;
    public float targetCheckRadius;
    [Range(0,1)]
    public float anticipationMovePercent;

    private Animator animator;
    private SphereCollider sphereCollider;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    public float maxSpeed;
    public float maxAcceleration;
    private float timer;
    private float anticipationMoveWaitTime;
    private float waitTime;
    private float animatorChangeRate;
    private float targetJumpHeight;
    private float jumpHeight;
    private AnimatorPropertyChanger animatorPropertyChanger;
    private Collider[] overlapTargets;

    protected override void MyAwake()
    {
        //TODO any kind of object pooling here?
        overlapTargets = new Collider[1];
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        sphereCollider = GetComponentInChildren<SphereCollider>();
        animatorChangeRate = 1 / anticipationMovePercent;
        animatorPropertyChanger = DoNothingChanger;
        SetJumpHeight(0);

        maxSpeed = navMeshAgent.speed;
        maxAcceleration = navMeshAgent.acceleration;
        jumpHeight = 0;
        targetJumpHeight = 0;
        timer = 0;
        waitTime = 0;
        anticipationMoveWaitTime = 0;
    }

    protected override void MyFixedUpdateWithDeltaTime(float myDeltaTime, float timeScale)
    {
        if (animatorPropertyChanger(myDeltaTime))
            animatorPropertyChanger = DoNothingChanger;

        navMeshAgent.speed = maxSpeed * timeScale;
        navMeshAgent.acceleration = maxAcceleration * timeScale;

        timer += myDeltaTime;
        if(!navMeshAgent.enabled)
        {
            if(timer>waitTime)
            {
                Vector3 targetPosition = GetTargetTransform();
                navMeshAgent.enabled = true;
                navMeshAgent.SetDestination(targetPosition);
                SetMoveState();
                timer = 0;
            }
            else if(timer > anticipationMoveWaitTime)
            {
                SetPreMoveState();
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
                SetEndMoveState();
                timer = 0;
            }
        }
    }

    private bool DoNothingChanger(float deltaTime)
    {
        return false;
    }

    private bool DecreaseChanger(float deltaTime)
    {
        bool doneChanging = false;
        jumpHeight -= animatorChangeRate * deltaTime;
        if (jumpHeight < targetJumpHeight)
        {
            jumpHeight = targetJumpHeight;
            doneChanging = true;
        }

        SetJumpHeight(jumpHeight);
        return doneChanging;
    }

    private bool IncreaseChanger(float deltaTime)
    {
        bool doneChanging = false;
        jumpHeight += animatorChangeRate * deltaTime;
        if (jumpHeight > targetJumpHeight)
        {
            jumpHeight = targetJumpHeight;
            doneChanging = true;
        }

        SetJumpHeight(jumpHeight);
        return doneChanging;
    }

    private void SetPreMoveState()
    {
        targetJumpHeight = -1;
        animatorPropertyChanger = DecreaseChanger;
    }

    private void SetMoveState()
    {
        targetJumpHeight = 1;
        animatorPropertyChanger = IncreaseChanger;
    }

    private void SetEndMoveState()
    {
        targetJumpHeight = 0;
        animatorPropertyChanger = DecreaseChanger;
    }

    private bool IsNavMeshFinished()
    {
        return navMeshAgent.remainingDistance != Mathf.Infinity && navMeshAgent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete && navMeshAgent.remainingDistance == 0;
    }

    private void SetJumpHeight(float value)
    {
        animator.SetFloat(JUMP_HEIGHT_HASH, value);
    }

    private Vector3 GetTargetTransform()
    {
        if (Physics.OverlapSphereNonAlloc(transform.position, targetCheckRadius, overlapTargets, targetLayer, QueryTriggerInteraction.Ignore) > 0)
            return overlapTargets[0].transform.position;

        return new Vector3(transform.position.x + GetSinglePosition(), transform.position.y, transform.position.z + GetSinglePosition());
    }

    private float GetSinglePosition()
    {
        return RandomNumberGeneratorUtils.unityRNG.GetValueInRange(-1 * targetCheckRadius, targetCheckRadius);
    }
}