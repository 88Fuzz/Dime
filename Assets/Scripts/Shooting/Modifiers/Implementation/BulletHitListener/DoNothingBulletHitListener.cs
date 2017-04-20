using UnityEngine;

/*
 * Default implementation of the BulletHitListener. No special actions will be done with the bullet.
 */
[CreateAssetMenu(fileName = "DoNothingBulletHitListener", menuName = "ScriptableObjects/Bullets/BulletHitListener/DoNothingBulletHitListener")]
public class DoNothingBulletHitListener : BulletHitListener
{
    public override bool OnEnemyHit(Bullet bullet, Hittable enemy)
    {
        //Do nothing and delete the bullet.
        return true;
    }

    public override bool OnEnemyKill(Bullet bullet, Hittable enemy)
    {
        //Do nothing and delete the bullet.
        return true;
    }

    public override void OnObjectHit(Bullet bullet, GameObject obj)
    {
        //Do Nothing
    }
}