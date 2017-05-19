using UnityEngine;

/*
 * Increase the damage of the bullets fired
 */
public class DamageUpgrade : Pickup
{
    protected override void PickedUp(Player player)
    {
        PlayerStats.IncrementValue(PlayerStats.Stat.SHOOT_DAMAGE_MULTIPLIER);
    }
}