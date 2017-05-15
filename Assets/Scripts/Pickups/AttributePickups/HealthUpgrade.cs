using UnityEngine;

/*
 * Increases player's max health
 */
public class HealthUpgrade : Pickup
{
    protected override void PickedUp(GameObject player)
    {
        PlayerStats.IncrementValue(PlayerStats.Stat.HEALTH);
        PlayerHittable playerHittable = player.GetComponent<PlayerHittable>();
        if (playerHittable)
            playerHittable.UpdateMaxHealth();
    }
}