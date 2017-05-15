using UnityEngine;

/*
 * Upgrades the bullet speed
 */
public class BulletSpeedUpgrade : Pickup
{
    protected override void PickedUp(GameObject player)
    {
        PlayerStats.IncrementValue(PlayerStats.Stat.BULLLET_SPEED);
    }
}