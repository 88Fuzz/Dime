using System.Collections.Generic;
using UnityEngine;

//TODO shouldn't be abstract anymore!! It should be using all the ScriptableObjects you created
//TODO, bullets should have a max amount of time to live
public abstract class Bullet : MonoBehaviour
{
    public BulletVelocityModifier velocityModifier;
    public BulletSizeModifier sizeModifier;
    public List<BulletHitListener> hitListeners;

    private Rigidbody bulletRigidbody;
    private int objectMask;
    private Vector3 previousPosition;
    private Renderer rendererComponent;

	public void Awake()
    {
        hitListeners = new List<BulletHitListener>();
        objectMask = LayerMask.GetMask("Object") | LayerMask.GetMask("Floor");
        Initalize();
        previousPosition = transform.position;
        rendererComponent = GetComponent<Renderer>();
        bulletRigidbody = GetComponent<Rigidbody>();
	}
	
	public void FixedUpdate()
    {
        int layerMask = objectMask | GetLayerMask();
        RaycastHit hitInfo;
        float maxDistance = Vector3.Distance(previousPosition, transform.position);
        float radius = GetRadius();
        //Doing a spherecast when the bullet is spawned does not return any results. I'm assuming that's because spherecast cannot determine a collision if an object overlaps in the initial position
        if(maxDistance == 0)
        {
            Collider[] colliders = new Collider[1];
            int collidersFound = Physics.OverlapSphereNonAlloc(previousPosition, radius, colliders, layerMask, QueryTriggerInteraction.Collide);
            //There should only be one collider found, since we are allocating a space for 1 collider.
            if(collidersFound != 0)
                ColliderHit(colliders[0]);
        }
        else if (Physics.SphereCast(previousPosition, radius, transform.position - previousPosition, out hitInfo, maxDistance, layerMask, QueryTriggerInteraction.Collide))
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
            float hitStrength = GetHitInformation();
            bool hitKilled = hittable.Hit(hitStrength);
            OnEnemyHit(hittable);
            if (hitKilled)
                OnEnemyKill(hittable);
        }
        else
            OnObjectHit(collider);
        Destroyed();
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

    //TODO once this is no longer abstract, make this private!!
    protected void SetRadius(float radius)
    {
        transform.localScale = Vector3.one * radius;
    }

    /*
     * Return the hit information associated with the bullet.
     */
    abstract protected float GetHitInformation();

    /*
     * The layermask returned will be OR'd with the Object layer to find collisions with objects in the world.
     */
    abstract protected int GetLayerMask();

    /*
     * Do any initialization needed for the bullet. Called during the Awake method.
     */
    abstract protected void Initalize();

    /*
     * The Bullet has hit something to make the stop/disapear. Do anything that needs to be done as an "end action" for the bullet life span.
     */
    abstract protected void Destroyed();

    /*
     * Return the velocity for when the bullet is first spawned.
     */
    abstract public Vector3 GetInitialVelocity();
}