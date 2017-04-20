using UnityEngine;

public class DamageUpgrade : Upgrade
{
    public float damageIncreaseMultiplyer = 1;
    protected override void UpgradePickedUp(GameObject player)
    {
        PlayerStats.shootDamageMultiplier += damageIncreaseMultiplyer;
    }
}