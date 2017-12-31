using UnityEngine;

public class FindTargetBullet : BasicBullet
{
    public float scanDistance;
    public LayerMask targetMask;
    public float damage;

    protected override void BulletFixedUpdate(float myDeltaTime, float timeScale)
    {
        //Do nothing
    }

    protected override void BulletInit()
    {
        Collider[] colliders = new Collider[1];
        if(Physics.OverlapSphereNonAlloc(transform.position, scanDistance, colliders, targetMask, QueryTriggerInteraction.Ignore)> 0)
        {
            Vector3 directionToTarget = colliders[0].transform.position - transform.position;
            directionToTarget.y = transform.position.y;
            velocity = directionToTarget.normalized * initialVelocity;
        }
    }

    protected override void ColliderHit(Collider collider, Hittable hittable)
    {
        if(hittable)
            hittable.Hit(damage);

        MyDestroy();
    }

    protected override LayerMask GetLayerMask()
    {
        return layerMask;
    }
}