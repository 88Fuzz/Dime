using UnityEngine;
using System.Collections.Generic;

/*
 * Responsible for deciding what kind of bullets and the modifiers placed on the bullets that should be spawned.
 */
[CreateAssetMenu(fileName = "BulletManager", menuName = "ScriptableObjects/Bullets/BulletManager", order = 1)]
public class BulletManager : ScriptableObject 
{
    /*
     * //TODO move somewhere else!
     * Holder class to organize a range with the bullet associated with the range.
     */
    private class BulletRange
    {
        public Range range;
        public Bullet bullet;
    }
    public List<BulletSpawnListener> bulletSpawnListeners;
    public List<BulletChanceInformation> bulletTypes;
    public List<BulletHitListener> hitListeners;
    public BulletSpawnNumberDecider bulletSpawnNumberDecider;
    public BulletVelocityModifier bulletVelocityModifier;
    public BulletSizeModifier bulletSizeModifier;

    private System.Random random;
    private int bulletChanceCount;
    private List<BulletRange> probabilityRange;

    public void OnEnable()
    {
        random = new System.Random();
        probabilityRange = new List<BulletRange>();
        CalculateBulletChances();
    }

    public Bullet[] GetBullets()
    {
        int numberOfBullets = bulletSpawnNumberDecider.GetNumberOfBulletsToSpawn();
        if (numberOfBullets <= 0)
            return new Bullet[0];

        Bullet[] bulletSpawnInformation = new Bullet[numberOfBullets];

        for(int i = 0; i < numberOfBullets; i++)
        {
            Bullet bullet = GetRandomBullet();
            bullet.SetBulletVelocityModifier(bulletVelocityModifier);
            bullet.SetBulletSizeModifier(bulletSizeModifier);
            bullet.AddBulletHitListeners(hitListeners);
            bulletSpawnInformation[i] = bullet;
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
        int bulletNumber = random.Next(bulletChanceCount) + 1;

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