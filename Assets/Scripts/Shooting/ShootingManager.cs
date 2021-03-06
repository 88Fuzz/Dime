﻿using System.Collections.Generic;
using UnityEngine;

/*
 * Responsible for spawning bullets when asked.
 */
public class ShootingManager : MyMonoBehaviour
{
    //There should also be a BulletSpawnPositionManager that will return the number of transforms to spawn bullets
    public List<Transform> bulletSpawnPositions;
    public ShootDelayModifier shootDelayModifier;
    public Player player;

    private float timer;
    private float shootDelay;
    private int bulletSpawnSelector;

    protected override void MyAwake()
    {
        timer = 0;
        bulletSpawnSelector = 0;

        //TODO, I like the idea of different ActionManagerss existing, So one may act on button clicks, while another acts on a X second tick to fire bullets
        ActionManager actionManager = Singleton<ActionManager>.Instance;
        actionManager.RegisterContinuousButtonListener(InputButton.PRIMARY_ATTACK, FireBullet);
        shootDelayModifier.InitModifier();
        shootDelay = shootDelayModifier.GetShootDelay(this);
    }

    protected override void MyFixedUpdateWithDeltaTime(float myDeltaTime, float timeScale)
    {
        timer += myDeltaTime;
    }

    public void FireBullet(InputButton button)
    {
        if (timer > shootDelay)
        {
            timer = 0;
            shootDelay = shootDelayModifier.GetShootDelay(this);

            Bullet[] spawnBullets = player.bulletManager.GetBullets();
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

        BulletManager bulletManager = player.bulletManager;
        spawnedBullet.player = player;
        spawnedBullet.hitInformation = bulletManager.hitInformationProvider.GetHitInformation();
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
        spawnedBullet.player = bullet.player;
        spawnedBullet.hitInformation = bullet.hitInformation;
        spawnedBullet.velocityModifier = bullet.velocityModifier;
        spawnedBullet.sizeModifier = bullet.sizeModifier;
        spawnedBullet.hitListeners = bullet.hitListeners;
        ApplyBulletSpawnListeners(spawnedBullet);

        return spawnedBullet;
    }

    private void ApplyBulletSpawnListeners(Bullet spawnedBullet)
    {
        foreach (BulletSpawnListener spawnListener in player.bulletManager.bulletSpawnListeners)
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