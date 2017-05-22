using UnityEngine;

/*
 * Returns the shoot delay associated with the PlayerStats
 */
[CreateAssetMenu(fileName = "PlayerStatShootDelay", menuName = "ScriptableObjects/Shooting/ShootDelayModifier/PlayerStatShootDelay")]
public class PlayerStatShootDelay : ShootDelayModifier
{
    public override void InitModifier()
    {
        //Do nothing
    }
    /*
     * Returns the shoot delay associated with the PlayerStats
     */
    public override float GetShootDelay(ShootingManager shootingManager)
    {
        return PlayerStats.GetCurrentValue(PlayerStats.Stat.SHOOT_DELAY);
    }
}