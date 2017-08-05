using UnityEngine;

/*
 * Basic bullet handles the hit detection as the bullet moves. It will also linear movement across FixedUpdates.
 */
public abstract class BasicBullet : MyMonoBehaviour
{
    private static readonly float MINIMUM_DISTANCE = .1f;
    public float initialVelocity;
    public bool useGravity;
    public LayerMask layerMask;
    public BulletHitInformation hitInformation;

    protected Vector3 velocity;

    private Rigidbody bulletRigidbody;
    private Vector3 previousPosition;
    private Collider[] colliders;

    /*
     * Sets the velocity of the bullet in the local z direction
     */
    public void CalculateForwardVelocity()
    {
        velocity = transform.TransformDirection(Vector3.forward * initialVelocity);
    }

    /*
     * Returns the scale of the transform. Bullets are assumed to be a sphere,
     * so a single value of the localScale is assumed to be the radius.
     */
    public float GetRadius()
    {
        return transform.localScale.x / 2;
    }

    /*
     * Bullets are assumed to be a sphere, so all components of the localScale will take the value of radius.
     */
    protected void SetRadius(float radius)
    {
        transform.localScale = Vector3.one * (radius * 2);
    }

    protected override void MyAwake()
    { 
        BulletInit();
        //TODO is GetComponent expensive to do everytime things are initialized?
        //TODO I could just make it a public variable to have set in the prefab?
        bulletRigidbody = GetComponent<Rigidbody>();
        //TODO some kind of object pooling
        colliders = new Collider[1];
        previousPosition = transform.position;
    }

    protected override void MyFixedUpdateWithDeltaTime(float myDeltaTime, float timeScale)
    {
        RaycastHit hitInfo;
        float distanceMoved = Vector3.Distance(previousPosition, transform.position);
        float radius = GetRadius();
        LayerMask mask = GetLayerMask();
        //Doing a spherecast when the bullet is spawned/doesn't move does not return any results. 
        //I'm assuming that's because spherecast cannot determine a collision if an object overlaps in the initial position
        if (distanceMoved <= MINIMUM_DISTANCE)
        {
            int collidersFound = Physics.OverlapSphereNonAlloc(previousPosition, radius, colliders, mask, QueryTriggerInteraction.Collide);
            //There should only be one collider found, since we are allocating a space for 1 collider.
            if (collidersFound != 0)
                ColliderHit(colliders[0]);
        }
        else if (Physics.SphereCast(previousPosition, radius, transform.position - previousPosition, out hitInfo, distanceMoved, mask, QueryTriggerInteraction.Collide))
        {
            ColliderHit(hitInfo.collider);
        }

        previousPosition = transform.position;
        Vector3 moveDistance = velocity * myDeltaTime;
        if (useGravity)
            moveDistance += Physics.gravity * myDeltaTime;

        bulletRigidbody.MovePosition(previousPosition + moveDistance);
        BulletFixedUpdate(myDeltaTime, timeScale);
    }

    private void ColliderHit(Collider collider)
    {
        Hittable hittable = collider.gameObject.GetComponent<Hittable>();
        if (hittable)
        {
            Vector3 hitPosition = collider.ClosestPointOnBounds(transform.position);
            //TODO object pooling
            ParticleSystem particleSystem = Instantiate<ParticleSystem>(hitInformation.ParticleSystem,hitPosition,Quaternion.identity,null);
            particleSystem.transform.LookAt(transform);
            Destroy(particleSystem, .5f);
        }

        ColliderHit(collider, collider.gameObject.GetComponent<Hittable>());
    }

    /*
     * Get the LayerMask for determining hit collisions
     */
    protected abstract LayerMask GetLayerMask();

    /*
     * Called when the bullet hits something. Hittable is not null if the collider has a Hittable component.
     */
    protected abstract void ColliderHit(Collider collider, Hittable hittable);

    /*
     * Called at the begininning of MyAwake
     */
    protected abstract void BulletInit();

    /*
     * Called at the end of MyFixedUpdateWithDeltaTime
     */
    protected abstract void BulletFixedUpdate(float myDeltaTime, float timeScale);
}