using UnityEngine;

public class BulletSpeedUpgrade : Upgrade
{
    protected override void UpgradePickedUp(GameObject player)
    {
        PlayerStats.IncrementValue(PlayerStats.Stat.BULLLET_SPEED);
    }
}