using UnityEngine;

public class BulletSpeedUpgrade : Upgrade
{
    public float speedIncrease = 10;

    protected override void UpgradePickedUp(GameObject player)
    {
        PlayerStats.shootSpeed += speedIncrease;
    }
}