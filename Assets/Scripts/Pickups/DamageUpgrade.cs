using UnityEngine;

public class DamageUpgrade : Upgrade
{
    protected override void UpgradePickedUp(GameObject player)
    {
        PlayerStats.IncrementValue(PlayerStats.Stat.SHOOT_DAMAGE_MULTIPLIER);
    }
}