using UnityEngine;

public class MovementUpgrade : Upgrade
{
    public float increaseMovementSpeed = 4;

    protected override void UpgradePickedUp(GameObject player)
    {
        PlayerStats.movementSpeed += increaseMovementSpeed;
    }
}