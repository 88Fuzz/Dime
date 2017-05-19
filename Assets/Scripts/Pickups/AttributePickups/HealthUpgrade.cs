using UnityEngine;

/*
 * Increases player's max health
 */
public class HealthUpgrade : Pickup
{
    protected override void PickedUp(Player player)
    {
        PlayerStats.IncrementValue(PlayerStats.Stat.HEALTH);
        player.hittable.UpdateMaxHealth();
    }
}