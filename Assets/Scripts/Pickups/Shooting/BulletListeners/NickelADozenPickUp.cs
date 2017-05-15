using UnityEngine;

/*
 * Adds the NickelADozen to the player's BulletManager
 */
public class NickelADozenPickUp : Pickup
{
    NickelADozen nickelADozen;

    protected override void PickedUp(GameObject player)
    {
        ShootingManager shootingManager = player.GetComponent<ShootingManager>();
        if(shootingManager)
        {
            shootingManager.bulletManager.hitListeners.Add(nickelADozen);
        }
    }
}