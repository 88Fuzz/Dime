using UnityEngine;

public class HealthUpgrade : Upgrade
{
    public float healthIncrease = 20;

    protected override void UpgradePickedUp(GameObject player)
    {
        PlayerStats.maxHealth += healthIncrease;
        PlayerHittable playerHittable = player.GetComponent<PlayerHittable>();
        if (playerHittable)
            playerHittable.UpdateMaxHealth();
    }
}