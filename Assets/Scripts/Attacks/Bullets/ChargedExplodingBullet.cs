using UnityEngine;

/*
 * The ChargedExplodingBullet will start small, grow to a specific size, then launches at a target.
 * When it launches at the target, it makes an assumption on where it will collide with the target.
 * Once the bullet gets to the collision point it grows in size before dispearing.
 */
public class ChargedExplodingBullet : BasicBullet
{
    private delegate void FixedUpdateAction(float myDeltaTime, float timeScale);
    private delegate void FinishAction();

    public float minimumTargetDistance;
    public float scanDistance;
    public float initalSize;
    public float movementSize;
    public float explosionSize;
    public float initialSizeChange;
    public float explosionSizeChange;
    public float explosionWaitTime;
    public float deflationWaitTime;
    public LayerMask targetMask;

    private LayerMask currentLayerMask;
    private Collider[] colliderResults;
    private FixedUpdateAction fixedUpdateAction;
    //Called whenever the FixedUpdateAction is "done" and needs to set some new state on the bullet.
    private FinishAction finishAction;
    private PhysicsUtils.InterceptData interceptData;
    private float sizeChanger;
    private float finalRadius;
    private float timer;
    private float waitTime;
    private float distanceToHitPoint;

    protected override void BulletInit()
    {
        //TODO some kind of object pooling
        colliderResults = new Collider[1];
        useGravity = false;
        currentLayerMask = targetMask;
        SetRadius(initalSize);
        SetSizeModifierVariables(movementSize, initialSizeChange, IncreaseSize, StartMoving);
        distanceToHitPoint = -1;
    }

    protected override void BulletFixedUpdate(float myDeltaTime, float timeScale)
    {
        fixedUpdateAction(myDeltaTime, timeScale);
    }

    protected override void ColliderHit(Collider collider)
    {
        Hittable hittable = collider.GetComponent<Hittable>();
        if (hittable)
            hittable.Hit(damage);
        //Do not do explosion if hitting anything other than a player
        else
            BulletDone();

        if (fixedUpdateAction == MoveBulletToExpectedCollisionPoint)
            StartExplosion();
    }

    private void DoNothingTimer(float myDeltaTime, float timeScale)
    {
        timer += myDeltaTime;
        if (timer > waitTime)
            finishAction();
    }

    private void IncreaseSize(float myDeltaTime, float timeScale)
    {
        float radius = GetRadius() + sizeChanger * myDeltaTime;
        if (radius > finalRadius)
        {
            radius = finalRadius;
            finishAction();
        }
        SetRadius(radius);
    }

    private void DecreaseSize(float myDeltaTime, float timeScale)
    {
        float radius = GetRadius() - sizeChanger * myDeltaTime;
        if (radius < finalRadius)
        {
            radius = finalRadius;
            finishAction();
        }
        SetRadius(radius);
    }

    private void MoveBulletToExpectedCollisionPoint(float myDeltaTime, float timeScale)
    {
        float distance = Vector3.Distance(transform.position, interceptData.HitPosition);
        velocity = GetVelocityFromDistance(distance);
        if(distance < minimumTargetDistance)
            StartExplosion();
    }

    /*
     * Ideally slerp or lerp would work here, but I want the velocity to drop off once it is extremely close to the
     * expected hit point. slerp and lerp smooth out the velocity too much.
     */
    private Vector3 GetVelocityFromDistance(float distance)
    {
        if(distance < 6)
        {
            if (distanceToHitPoint < 0)
                distanceToHitPoint = Vector3.Distance(transform.position, interceptData.HitPosition);
            return Vector3.Slerp(interceptData.Velocity, Vector3.zero, 1 - (distance / distanceToHitPoint));
        }

        return interceptData.Velocity;
    }

    private void BulletDone()
    {
        MyDestroy();
    }

    private void StartMoving()
    {
        if(Physics.OverlapSphereNonAlloc(transform.position, scanDistance, colliderResults, targetMask, QueryTriggerInteraction.Ignore) > 0)
        {
            Collider collider = colliderResults[0];
            Vector3 targetPosition = collider.gameObject.transform.position;
            Rigidbody targetRigidbody = collider.GetComponent<Rigidbody>();
            if(!targetRigidbody)
            {
                //Debug.Log("No rigidbody");
                StartExplosion();
                return;
            }

            interceptData = PhysicsUtils.FindInterceptingData(transform.position, initialVelocity, targetPosition, targetRigidbody.velocity);
            if(!interceptData.CanHit)
            {
                //Debug.Log("No hitPoint");
                StartExplosion();
                return;
            }

            //Debug.Log("Everything is good to go! expected hit point " + interceptData.HitPosition + " bullet velocity " + interceptData.Velocity);
            //Debug.Log("\tcurrent target position " + targetPosition + " with a velocity " + targetRigidbody.velocity);
            //TODO do a raycast to see if anything is in the way of hitting the player?
            ActuallyStartMoving();
            return;
        }

        StartExplosion();
    }

    private void ActuallyStartMoving()
    {
        currentLayerMask = layerMask;
        fixedUpdateAction = MoveBulletToExpectedCollisionPoint;
    }

    private void StartExplosion()
    {
        velocity = Vector3.zero;
        //Add a short delay before explosion
        SetWaitTimer(explosionWaitTime, ActuallyStartExplosion);
    }

    private void WaitBeforeShrinking()
    {
        SetWaitTimer(deflationWaitTime, DecreaseToNothing);
    }

    private void ActuallyStartExplosion()
    {
        currentLayerMask = targetMask;
        //First decrease the bullet size to make the explosion look better? Hopefully.
        SetSizeModifierVariables(movementSize * .5f, explosionSizeChange, DecreaseSize, ExpandExplosion);
    }

    private void ExpandExplosion()
    {
        SetSizeModifierVariables(explosionSize, explosionSizeChange, IncreaseSize, WaitBeforeShrinking);
    }

    private void DecreaseToNothing()
    {
        SetSizeModifierVariables(0, explosionSizeChange, DecreaseSize, BulletDone);
    }

    private void SetSizeModifierVariables(float finalRadius, float sizeChanger, FixedUpdateAction fixedUpdateAction, FinishAction finishAction)
    {
        this.finalRadius = finalRadius;
        this.sizeChanger = sizeChanger;
        this.fixedUpdateAction = fixedUpdateAction;
        this.finishAction = finishAction;
    }

    private void SetWaitTimer(float waitTime, FinishAction finishAction)
    {
        timer = 0;
        fixedUpdateAction = DoNothingTimer;
        this.waitTime = waitTime;
        this.finishAction = finishAction;
    }

    protected override LayerMask GetLayerMask()
    {
        return currentLayerMask;
    }
}