using System.Collections.Generic;
using UnityEngine;

//TODO, bullets should have a max amount of time to live
public class Bullet : MonoBehaviour
{
    public BulletManager bulletManager;
    public BulletVelocityModifier velocityModifier;
    public BulletSizeModifier sizeModifier;
    public List<BulletHitListener> hitListeners;
    public float damage;

    private Rigidbody bulletRigidbody;
    private int hittableMask;
    private Vector3 previousPosition;

	public void Awake()
    {
        hitListeners = new List<BulletHitListener>();
        hittableMask = LayerMask.GetMask("Object") | LayerMask.GetMask("Floor") | LayerMask.GetMask("Enemy");
        SetRadius(PlayerStats.shootSize);
        previousPosition = transform.position;
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletRigidbody.velocity = transform.TransformDirection(Vector3.forward * PlayerStats.shootSpeed);
    }
	
	public void FixedUpdate()
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
        bulletRigidbody.velocity = velocityModifier.ChangeVelocity(bulletRigidbody.velocity);
        float previousRadius = GetRadius();
        float newRadius = sizeModifier.ChangeSize(previousRadius);
        if(newRadius != previousRadius)
            SetRadius(newRadius);
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
        if (hittable)
        {
            bool hitKilled = hittable.Hit(damage);
            OnEnemyHit(hittable);
            if (hitKilled)
                OnEnemyKill(hittable);
        }
        else
            OnObjectHit(collider);
        Destroy(gameObject);
    }

    private void OnEnemyHit(Hittable hittable)
    {
        foreach(BulletHitListener hitListener in hitListeners)
        {
            hitListener.OnEnemyHit(this, hittable);
        }
    }

    private void OnEnemyKill(Hittable hittable)
    {
        foreach(BulletHitListener hitListener in hitListeners)
        {
            hitListener.OnEnemyKill(this, hittable);
        }
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