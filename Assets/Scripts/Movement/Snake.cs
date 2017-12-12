using UnityEngine;

public class Snake : MyMonoBehaviour
{
    public Range moveDistance;
    public float moveSpeed;
    public float followDistance;

    private Snake head;
    private Vector3 previousPosition;
    private Vector3 targetPosition;
    private Rigidbody rb;

    protected override void MyAwake()
    {
        head = null;
        previousPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    protected override void MyFixedUpdateWithDeltaTime(float myDeltaTime, float timeScale)
    {
        float distancedMoved = Vector3.Distance(previousPosition, transform.position);
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        if (distanceToTarget <= .2f || (distancedMoved <= .05f && timeScale == 1))
            CalculateNewTargetPosition();

        Vector3 velocity = PhysicsUtils.GetVelocityVector(transform.position, targetPosition, moveSpeed, true);
        rb.position = PhysicsUtils.GetNewPosition(transform.position, velocity, myDeltaTime);
        rb.velocity = Vector3.zero;

        previousPosition = transform.position;
    }

    public Snake Head
    {
        get { return head; }
        set
        {
            if(head != null)
            {
                head.DeregisterOnDestroyListener(SnakeNodeKilled);
            }
            if(value != null)
            {
                value.RegisterOnDestroyListener(SnakeNodeKilled);
            }

            CalculateNewTargetPosition();
            head = value;
        }
    }

    public void SnakeNodeKilled(GameObject snakeNode)
    {
        if (head.gameObject == snakeNode)
            head = null;
    }

    private void CalculateNewTargetPosition()
    {
        if(head != null)
        {
            Vector3 headTarget = head.GetTargetPosition();
            Vector3 followOffest = (head.transform.position - headTarget).normalized * followDistance;
            targetPosition = head.transform.position + followOffest;
            return;
        }
        float moveAmount = RandomNumberGeneratorUtils.unityRNG.GetValueInRange(moveDistance);
        targetPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        switch(RandomNumberGeneratorUtils.unityRNG.GetValueInRange(0,4))
        {
            case 0:
                targetPosition.z = targetPosition.z + moveAmount;
                break;
            case 1:
                targetPosition.z = targetPosition.z - moveAmount;
                break;
            case 2:
                targetPosition.x = targetPosition.x + moveAmount;
                break;
            case 3:
                targetPosition.x = targetPosition.x - moveAmount;
                break;
                //TODO move diagonally?
            default:
                Debug.LogError("Oh shit, shouldn't be here :(");
                break;
        }
    }

    private Vector3 GetTargetPosition()
    {
        return targetPosition;
    }
}