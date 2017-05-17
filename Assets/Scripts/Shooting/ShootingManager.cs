using System.Collections.Generic;
using UnityEngine;

/*
 * Responsible for spawning bullets when asked.
 */
public class ShootingManager : MonoBehaviour
{
    //There should also be a BulletSpawnPositionManager that will return the number of transforms to spawn bullets
    public List<Transform> bulletSpawnPositions;
    public BulletManager bulletManager;
    public float shootDelay;

    private float timer;
    private int bulletSpawnSelector;

    public void Awake()
    {
        timer = 0;
        bulletSpawnSelector = 0;

        //TODO, I like the idea of different ActionManagerss existing, So one may act on button clicks, while another acts on a X second tick
        ActionManager actionManager = Singleton<ActionManager>.Instance;
        actionManager.RegisterContinuousButtonListener(InputButton.PRIMARY_ATTACK, FireBullet);
    }
    
    public void FixedUpdate()
    {
        timer += Time.deltaTime;
    }

    public void FireBullet(InputButton button)
    {
        if(button == InputButton.PRIMARY_ATTACK && timer > shootDelay)
        {
            timer = 0;
            Bullet[] spawnBullets = bulletManager.GetBullets();
            foreach(Bullet bullet in spawnBullets)
            {
                //TODO some kind of object pooling
                SpawnNewBullet(bullet);
            }
        }
    }

    /*
     * Spawns a new bullet with new configurations based on the BulletManager. New bullet is returned.
     */
    public Bullet SpawnNewBullet(Bullet bullet)
    {
        Transform nextBulletPositionTransform = GetNextSpawnPosition();
        //TODO some kind of object pooling
        Bullet spawnedBullet = Instantiate(bullet, nextBulletPositionTransform.position, nextBulletPositionTransform.rotation) as Bullet;
        spawnedBullet.shootingManager = this;
        spawnedBullet.bulletManager = bulletManager;
        spawnedBullet.damage = bulletManager.hitInformationProvider.GetHitInformation();
        spawnedBullet.SetBulletVelocityModifier(bulletManager.bulletVelocityModifier);
        spawnedBullet.SetBulletSizeModifier(bulletManager.bulletSizeModifier);
        spawnedBullet.AddBulletHitListeners(bulletManager.hitListeners);
        ApplyBulletSpawnListeners(spawnedBullet);

        return spawnedBullet;
    }

    /*
     * Spawns an exact copy of the bullet passed in. Bullet returned is the new bullet.
     */
    public Bullet SpawnBulletCopy(Bullet bullet)
    {
        //TODO some kind of object pooling
        Bullet spawnedBullet = Instantiate(bullet, bullet.transform.position, bullet.transform.rotation) as Bullet;
        spawnedBullet.shootingManager = this;
        spawnedBullet.bulletManager = bullet.bulletManager;
        spawnedBullet.damage = bullet.damage;
        spawnedBullet.velocityModifier = bullet.velocityModifier;
        spawnedBullet.sizeModifier = bullet.sizeModifier;
        spawnedBullet.hitListeners = bullet.hitListeners;
        ApplyBulletSpawnListeners(spawnedBullet);

        return spawnedBullet;
    }

    private void ApplyBulletSpawnListeners(Bullet spawnedBullet)
    {
        foreach (BulletSpawnListener spawnListener in bulletManager.bulletSpawnListeners)
        {
            spawnListener.OnBulletSpawn(spawnedBullet);
        }
    }

    //TODO make this modular as well, could have a random position, vs sequentially, vs some pretty pattern?
    private Transform GetNextSpawnPosition()
    {
        Transform transform = bulletSpawnPositions[bulletSpawnSelector++];
        if (bulletSpawnSelector >= bulletSpawnPositions.Count)
            bulletSpawnSelector = 0;

        return transform;
    }
}