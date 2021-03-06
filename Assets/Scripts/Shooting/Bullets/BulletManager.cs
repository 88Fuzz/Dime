﻿using UnityEngine;
using System.Collections.Generic;

/*
 * Responsible for deciding what kind of bullets and the modifiers placed on the bullets that should be spawned.
 */
 //TODO Bullets should use an Item pool at some point in the future to spawn the bullet GameObjects
[CreateAssetMenu(fileName = "BulletManager", menuName = "ScriptableObjects/Bullets/BulletManager", order = 1)]
public class BulletManager : ScriptableObject 
{
    /*
     * //TODO move somewhere else!
     * Holder class to organize a range with the bullet associated with the range.
     */
    private struct BulletRange
    {
        public Range range;
        public Bullet bullet;
    }
    public Player player;
    public List<BulletChanceInformation> bulletTypes;
    public BulletSpawnNumberDecider bulletSpawnNumberDecider;

    public CommonBulletModifiers commonBulletModifiers;
    public BulletHitInformationProvider hitInformationProvider;
    public BulletVelocityModifier bulletVelocityModifier;
    public BulletSizeModifier bulletSizeModifier;
    public List<BulletHitListener> hitListeners;
    public List<BulletSpawnListener> bulletSpawnListeners;

    private int bulletChanceCount;
    private List<BulletRange> probabilityRange;

    public void OnEnable()
    {
        probabilityRange = new List<BulletRange>();
        CalculateBulletChances();
    }

    public Bullet[] GetBullets()
    {
        int numberOfBullets = bulletSpawnNumberDecider.GetNumberOfBulletsToSpawn();
        if (numberOfBullets <= 0)
            return new Bullet[0];

        //TODO object pooling
        Bullet[] bulletSpawnInformation = new Bullet[numberOfBullets];

        for(int i = 0; i < numberOfBullets; i++)
        {
            bulletSpawnInformation[i] = GetRandomBullet();
        }

        return bulletSpawnInformation;
    }

    public void AddBulletChance(BulletChanceInformation bulletChance)
    {
        bulletTypes.Add(bulletChance);
        //TODO recalculating all the bulletChances again is dumb and inefficient, at high list size
        CalculateBulletChances();
    }

    public void RemoveBulletChance(BulletChanceInformation bulletChance)
    {
        //TODO implement
    }

    private void CalculateBulletChances()
    {
        bulletChanceCount = 0;
        probabilityRange.Clear();
        foreach(BulletChanceInformation bulletType in bulletTypes)
        {
            BulletRange bulletRange = new BulletRange();
            bulletRange.range = new Range(bulletChanceCount + 1, bulletChanceCount + bulletType.chance);
            bulletRange.bullet = bulletType.bullet;
            probabilityRange.Add(bulletRange);
            bulletChanceCount += bulletType.chance;
        }
    }

    private Bullet GetRandomBullet()
    {
        int bulletNumber = RandomNumberGeneratorUtils.unityRNG.GetValueInRange(0, bulletChanceCount) + 1;

        foreach(BulletRange bulletRange in probabilityRange)
        {
            if(bulletNumber >= bulletRange.range.min && bulletNumber <= bulletRange.range.max)
            {
                return bulletRange.bullet;
            }
        }

        Debug.Log("Could not find bullet when getting random number. Figure out what's going on please.");
        return bulletTypes[0].bullet;
    }
}