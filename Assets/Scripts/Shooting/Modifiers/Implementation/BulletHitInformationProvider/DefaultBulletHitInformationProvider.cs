using UnityEngine;
/*
 * Start with a base damage and apply the PlayerStats multiplyer to the value.
 */
[CreateAssetMenu(fileName = "DefaultBulletHitInformationProvider", menuName = "ScriptableObjects/Bullets/BulletHitInformationProvider/DefaultBulletHitInformationProvider")]
public class DefaultBulletHitInformationProvider : BulletHitInformationProvider
{
    public float baseDamage;
    public override float GetHitInformation()
    {
        return baseDamage * PlayerStats.shootDamageMultiplier;
    }
}