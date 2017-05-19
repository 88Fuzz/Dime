using UnityEngine;

/*
 * Adds the NickelADozen to the player's BulletManager
 */
public class NickelADozenPickup : Pickup
{
    NickelADozen nickelADozen;

    protected override void PickedUp(Player player)
    {
        player.bulletManager.hitListeners.Add(nickelADozen);
    }
}