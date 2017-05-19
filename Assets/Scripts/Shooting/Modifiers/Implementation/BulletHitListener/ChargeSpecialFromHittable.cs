using UnityEngine;

/*
 * Applies the Hittable's special charge to the player.
 */
[CreateAssetMenu(fileName = "ChargeSpecialFromHittable", menuName = "ScriptableObjects/Bullets/BulletHitListener/ChargeSpecialFromHittable")]
public class ChargeSpecialFromHittable: BulletHitListener
{
    /*
     * Do nothing
     */
    public override bool OnEnemyHit(Bullet bullet, Hittable enemy)
    {
        //Do nothing
        return true;
    }

    /*
     * Applies the Hittable's special charge to the player.
     */
    public override bool OnEnemyKill(Bullet bullet, Hittable enemy)
    {
        bullet.player.specialManager.IncreaseCharge(enemy.specialCharge);
        return true;
    }

    /*
     * Do nothing
     */
    public override void OnObjectHit(Bullet bullet, GameObject obj)
    {
        //Do nothing
    }
}