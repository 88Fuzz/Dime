﻿using System.Collections.Generic;
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
            List<BulletSpawnListener> bulletSpawnListeners = bulletManager.bulletSpawnListeners;
            foreach(Bullet bullet in spawnBullets)
            {
                Transform nextBulletPositionTransform = GetNextSpawnPosition();
                Bullet spawnedBullet = Instantiate(bullet, nextBulletPositionTransform.position, nextBulletPositionTransform.rotation) as Bullet;
                foreach(BulletSpawnListener spawnListener in bulletSpawnListeners)
                {
                    spawnListener.OnBulletSpawn(spawnedBullet);
                }
                //TODO this should be moved to a spawnListener!!
                Rigidbody bulletRigidbody = spawnedBullet.GetComponent<Rigidbody>();
                if(bulletRigidbody)
                {
                    bulletRigidbody.velocity = nextBulletPositionTransform.TransformDirection(spawnedBullet.GetInitialVelocity());
                }
            }
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