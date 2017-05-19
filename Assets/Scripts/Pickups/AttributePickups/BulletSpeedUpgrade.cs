using UnityEngine;

/*
 * Upgrades the bullet speed
 */
public class BulletSpeedUpgrade : Pickup
{
    protected override void PickedUp(Player player)
    {
        PlayerStats.IncrementValue(PlayerStats.Stat.BULLET_SPEED);
    }
}
