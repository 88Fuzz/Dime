using System.Collections.Generic;
using UnityEngine;

//TODO, bullets should have a max amount of time to live
public class Bullet : BasicBullet
{
    public Player player;
    public BulletVelocityModifier velocityModifier;
    public BulletSizeModifier sizeModifier;
    public List<BulletHitListener> hitListeners;

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

    protected override void BulletInit()
    {
        //TODO some kind of object pooling
        hitListeners = new List<BulletHitListener>(10);
        SetRadius(PlayerStats.GetCurrentValue(PlayerStats.Stat.BULLET_SIZE));
        initialVelocity = PlayerStats.GetCurrentValue(PlayerStats.Stat.BULLET_SPEED);
        CalculateForwardVelocity();
    }

    protected override void BulletFixedUpdate(float myDeltaTime, float timeScale)
    {
        float previousRadius = GetRadius();
        float newRadius = sizeModifier.ChangeSize(this, myDeltaTime, timeScale);
        if (newRadius != previousRadius)
            SetRadius(newRadius);
    }

    protected override void ColliderHit(Collider collider, Hittable hittable)
    {
        bool shouldDelete = true;
        if (hittable)
        {
            bool hitKilled = hittable.Hit(hitInformation.Damage);
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

    protected override LayerMask GetLayerMask()
    {
        return layerMask;
    }
}