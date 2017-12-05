using UnityEngine;

public class MobCircler : MyMonoBehaviour
{
    public float moveSpeed;
    public Vector3 targetLocation;
    public Transform lookAtTarget;
    public float targetYOffset;
    public float minimumDistance;

    private Rigidbody rb;

    protected override void MyAwake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected override void MyFixedUpdateWithDeltaTime(float myDeltaTime, float timeScale)
    {
        Vector3 differenceToTarget = targetLocation - transform.position;
        differenceToTarget.y = 0;
        Vector3 velocity = moveSpeed * differenceToTarget.normalized;
        Vector3 moveDistance = velocity * myDeltaTime;
        Vector3 newPosition = transform.position + moveDistance;
        newPosition.y = targetYOffset;
        //Set the velocity to 0 because, collisions with the moving box messes keeps the velocity around.
        rb.velocity = Vector3.zero;
        rb.MovePosition(Vector3.Lerp(transform.position, newPosition, .5f));
        //TODO this look at should be smoothed out
        transform.LookAt(lookAtTarget);
    }
}