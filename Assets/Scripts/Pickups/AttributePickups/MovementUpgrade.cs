using UnityEngine;

/*
 * Increase player's movement speed
 */
public class MovementUpgrade : Pickup
{
    protected override void PickedUp(Player player)
    {
        PlayerStats.IncrementValue(PlayerStats.Stat.MOVEMENT_SPEED);
    }
}