using UnityEngine;
/*
 * Start with a base damage and apply the PlayerStats multiplyer to the value.
 */
[CreateAssetMenu(fileName = "DefaultBulletHitInformationProvider", menuName = "ScriptableObjects/Bullets/BulletHitInformationProvider/DefaultBulletHitInformationProvider")]
public class DefaultBulletHitInformationProvider : BulletHitInformationProvider
{
    protected override float GetFallbackHitInformation()
    {
        float random = RandomNumberGeneratorUtils.unityRNG.GetValueInRange(0f, 200f);

        if (random < PlayerStats.shootCritChance)
            return GetDamageValue(DamageModifier.CRIT);

        random -= 100;
        if (random >= 0 && random < PlayerStats.shootGlanceChance)
            return GetDamageValue(DamageModifier.GLANCE);

        return GetDamageValue(DamageModifier.NONE);
    }
}