using UnityEngine;

public class MovementUpgrade : Upgrade
{
    protected override void UpgradePickedUp(GameObject player)
    {
        PlayerStats.IncrementValue(PlayerStats.Stat.MOVEMENT_SPEED);
    }
}