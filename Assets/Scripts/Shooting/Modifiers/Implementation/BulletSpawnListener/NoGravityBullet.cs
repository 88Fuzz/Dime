using UnityEngine;

/*
 * Turns of gravity so tthat the range of the bullet is infinite.
 */
[CreateAssetMenu(fileName = "NoGravityBullet", menuName = "ScriptableObjects/Bullets/BulletSpawnListener/NoGravityBullet")]
public class NoGravityBullet : BulletSpawnListener
{
    /*
     * Turns of gravity so tthat the range of the bullet is infinite.
     */
    public override void OnBulletSpawn(Bullet bullet)
    {
        bullet.useGravity = false;
    }
}