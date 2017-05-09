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
        float damage = baseDamage;
        float random = RandomNumberGeneratorUtils.unityRNG.GetValueInRange(0f, 200f);

        if (random < PlayerStats.shootCritChance)
            damage *= PlayerStats.shootCritDamageMultiplier;

        random -= 100;
        if (random >= 0 && random < PlayerStats.shootGlanceChance)
            damage *= PlayerStats.shootGlanceDamageMultiplier;

        return damage * PlayerStats.shootDamageMultiplier;
    }

    private float GetRandomPercent()
    {
        return RandomNumberGeneratorUtils.unityRNG.GetValueInRange(0f, 200f);
    }
}