using UnityEngine;

public class HealthUpgrade : Upgrade
{
    protected override void UpgradePickedUp(GameObject player)
    {
        PlayerStats.IncrementValue(PlayerStats.Stat.HEALTH);
        PlayerHittable playerHittable = player.GetComponent<PlayerHittable>();
        if (playerHittable)
            playerHittable.UpdateMaxHealth();
    }
}