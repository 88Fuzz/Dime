using UnityEngine;
/*
 * Collection of physics functions.
 */
public class PhysicsUtils
{
    /*
     * Collection of Intercept data, used when calling FindInterceptingData.
     */
    public struct InterceptData
    {
        private Vector3 _velocity;
        private Vector3 _hitPosition;
        private bool _canHit;

        public InterceptData(Vector3 velocity, Vector3 hitPosition, bool canHit)
        {
            _velocity = velocity;
            _hitPosition = hitPosition;
            _canHit = canHit;
        }

        public Vector3 Velocity
        {
            get { return _velocity; }
        }

        public Vector3 HitPosition
        {
            get { return _hitPosition; }
        }

        public bool CanHit
        {
            get { return _canHit; }
        }
    }

    /*
     * Given a target position, target velocity, bullet position, and bullet speed.
     * Returns the velocity the bullet needs to travel to hit the target.
     * It will also return the position at which the bullet and target will hit.
     * If the bullet cannot travel fast enough to hit the target, a boolean value returned will indicate it cannot be reached.
     * 
     * This code was taken and modified by http://danikgames.com/blog/moving-target-intercept-in-3d/
     * and http://danikgames.com/blog/how-to-intersect-a-moving-target-in-2d/
     *
     *  targetOrthogonalVelocity
     *   |
     *   v
     * 
     *   ^...7   <-targetVelocity
     *   |  /.
     *   | / .
     *   |/  .
     *   t--->   <-targetTangentialVelocity
     * 
     * 
     *   b--->   <-bulletTangentialVelocity
     * 
     */
    public static InterceptData FindInterceptingData(Vector3 bulletPosition, float bulletSpeed, Vector3 targetPosition, Vector3 targetVelocity)
    {
        Vector3 directionToTarget = Vector3.Normalize(targetPosition - bulletPosition);
        /*
         * There is a weird piece of behavior here. If the targetVelocity is towards directionToTarget then Vector3.Dot(targetVelocity, directionToTarget)
         * will be < 0. Which messes with the targetOrthogonalVelocity as it flips the sign. If you take the abs of the dot product the hit point is
         * calculated correctly, but the velocity is super slow. 
         * 
         * At the time of writing this, I think it is best to leave the sign negative and deal with an incorrect hit point if the player is running directly at
         * the object firing the bullet.
         */
        Vector3 targetOrthogonalVelocity = Vector3.Dot(targetVelocity, directionToTarget) * directionToTarget;
        Vector3 targetTangentialVelocity = targetVelocity - targetOrthogonalVelocity;
        Vector3 bulletTangentialVelocity = targetTangentialVelocity;

        float bulletVelocityNeeded = bulletTangentialVelocity.magnitude;
        if(bulletVelocityNeeded > bulletSpeed)
        {
            //Bullet is too slow to intercept target.
            //Fire at the target's current position, because why not?
            return new InterceptData(directionToTarget * bulletSpeed, Vector3.zero, false);
        }
        float bulletOrthogonalSpeed = Mathf.Sqrt(bulletSpeed * bulletSpeed - bulletVelocityNeeded * bulletVelocityNeeded);

        // Add the tangential and orthogonal velocities to get the final velocity.
        Vector3 bulletOrthogonalVelocity = directionToTarget * bulletOrthogonalSpeed;
        Vector3 bulletVelocity = bulletOrthogonalVelocity + bulletTangentialVelocity;

        //Find the point of collision
        float timeToCollision = (bulletPosition - targetPosition).magnitude / (bulletOrthogonalVelocity.magnitude - targetOrthogonalVelocity.magnitude);
        Vector3 collisionPoint = bulletPosition + bulletVelocity * timeToCollision;

        return new InterceptData(bulletVelocity, collisionPoint, true);
    }
}