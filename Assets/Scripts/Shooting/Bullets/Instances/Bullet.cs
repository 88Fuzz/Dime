using System.Collections.Generic;
using UnityEngine;

//TODO, bullets should have a max amount of time to live
public class Bullet : MyMonoBehaviour
{
    public Player player;
    public BulletVelocityModifier velocityModifier;
    public BulletSizeModifier sizeModifier;
    public List<BulletHitListener> hitListeners;
    public float damage;

    private bool useGravity;
    private Rigidbody bulletRigidbody;
    private int hittableMask;
    private Vector3 previousPosition;
    private Vector3 velocity;

	protected override void MyAwake()
    {
        gameObject.name = System.Guid.NewGuid().ToString();
        hitListeners = new List<BulletHitListener>(10);
        hittableMask = LayerMask.GetMask("Object") | LayerMask.GetMask("Floor") | LayerMask.GetMask("Enemy");
        SetRadius(PlayerStats.GetCurrentValue(PlayerStats.Stat.BULLET_SIZE));
        previousPosition = transform.position;
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletRigidbody.useGravity = false;
        useGravity = true;
        velocity = CalculateForwardVelocity();
    }
	
    protected override void MyFixedUpdateWithDeltaTime(float myDeltaTime, float timeScale)
    {
        RaycastHit hitInfo;
        float maxDistance = Vector3.Distance(previousPosition, transform.position);
        float radius = GetRadius();
        //Doing a spherecast when the bullet is spawned does not return any results. I'm assuming that's because spherecast cannot determine a collision if an object overlaps in the initial position
        if(maxDistance == 0)
        {
            Collider[] colliders = new Collider[1];
            int collidersFound = Physics.OverlapSphereNonAlloc(previousPosition, radius, colliders, hittableMask, QueryTriggerInteraction.Collide);
            //There should only be one collider found, since we are allocating a space for 1 collider.
            if(collidersFound != 0)
                ColliderHit(colliders[0]);
        }
        else if (Physics.SphereCast(previousPosition, radius, transform.position - previousPosition, out hitInfo, maxDistance, hittableMask, QueryTriggerInteraction.Collide))
        {
            ColliderHit(hitInfo.collider);
        }

        previousPosition = transform.position;
        velocity = velocityModifier.ChangeVelocity(velocity);
        Vector3 moveDistance = velocity * myDeltaTime;
        //TODO this gravity seems weird. Is it wrong?
        if (useGravity)
            moveDistance += Physics.gravity * myDeltaTime;
        bulletRigidbody.MovePosition(bulletRigidbody.position + moveDistance);

        float previousRadius = GetRadius();
        float newRadius = sizeModifier.ChangeSize(previousRadius);
        if(newRadius != previousRadius)
            SetRadius(newRadius);
	}

    public void SetGravity(bool gravity)
    {
        useGravity = gravity;
    }

    /*
     * Sets the velocity of the bullet in the local z direction
     */
    public Vector3 CalculateForwardVelocity()
    {
        return transform.TransformDirection(Vector3.forward * PlayerStats.GetCurrentValue(PlayerStats.Stat.BULLET_SPEED));
    }

    public void SetBulletVelocityModifier(BulletVelocityModifier modifier)
    {
        if(velocityModifier == null || velocityModifier.CanBeRemoved())
        {
            velocityModifier = modifier;
        }
    }

    public void SetBulletSizeModifier(BulletSizeModifier modifier)
    {
        if(sizeModifier == null || sizeModifier.CanBeRemoved())
        {
            sizeModifier = modifier;
        }
    }

    public void AddBulletHitListeners(List<BulletHitListener> hitListeners)
    {
        this.hitListeners.AddRange(hitListeners);
    }

    private void ColliderHit(Collider collider)
    {
        Hittable hittable = collider.GetComponent<Hittable>();
        bool shouldDelete = true;
        if (hittable)
        {
            bool hitKilled = hittable.Hit(damage);
            if (hitKilled)
                shouldDelete = OnEnemyKill(hittable);
            else 
                shouldDelete = OnEnemyHit(hittable);
        }
        else
        {
            OnObjectHit(collider);
        }

        if(shouldDelete)
            MyDestroy();
    }

    private bool OnEnemyHit(Hittable hittable)
    {
        bool shouldDelete = CommonOnEnemyHit(hittable);
        foreach(BulletHitListener hitListener in hitListeners)
        {
            bool shouldDeleteResponse = hitListener.OnEnemyHit(this, hittable);
            //If there's a listener that says the bullet should not be deleted, that should trump all other responses.
            if (shouldDelete)
                shouldDelete = shouldDeleteResponse;
        }

        return shouldDelete;
    }

    private bool CommonOnEnemyHit(Hittable hittable)
    {
        bool shouldDelete = true;
        foreach(BulletHitListener hitListener in player.bulletManager.commonBulletModifiers.commonHitListeners)
        {
            bool shouldDeleteResponse = hitListener.OnEnemyHit(this, hittable);
            //If there's a listener that says the bullet should not be deleted, that should trump all other responses.
            if (shouldDelete)
                shouldDelete = shouldDeleteResponse;
        }

        return shouldDelete;
    }

    private bool OnEnemyKill(Hittable hittable)
    {
        bool shouldDelete = CommonOnEnemyKill(hittable);
        foreach(BulletHitListener hitListener in hitListeners)
        {
            bool shouldDeleteResponse = hitListener.OnEnemyKill(this, hittable);
            //If there's a listener that says the bullet should not be deleted, that should trump all other responses.
            if (shouldDelete)
                shouldDelete = shouldDeleteResponse;
        }

        return shouldDelete;
    }

    private bool CommonOnEnemyKill(Hittable hittable)
    {
        bool shouldDelete = true;
        foreach(BulletHitListener hitListener in player.bulletManager.commonBulletModifiers.commonHitListeners)
        {
            bool shouldDeleteResponse = hitListener.OnEnemyKill(this, hittable);
            //If there's a listener that says the bullet should not be deleted, that should trump all other responses.
            if (shouldDelete)
                shouldDelete = shouldDeleteResponse;
        }

        return shouldDelete;
    }

    private void OnObjectHit(Collider collider)
    {
        foreach(BulletHitListener hitListener in hitListeners)
        {
            hitListener.OnObjectHit(this, collider.gameObject);
        }
    }

    private float GetRadius()
    {
        //Bullets are assumed to be perfect spheres, so returning any of the x,y,z coordinates will be fine.
        return transform.localScale.x;
    }

    private void SetRadius(float radius)
    {
        transform.localScale = Vector3.one * radius;
    }
}
