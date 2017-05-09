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

        if (GetRandomPercent() < PlayerStats.shootCritChance)
            damage *= PlayerStats.shootCritDamageMultiplier;
        else if(GetRandomPercent() < PlayerStats.shootGlanceChance)
            damage *= PlayerStats.shootGlanceDamageMultiplier;

        return damage * PlayerStats.shootDamageMultiplier;
    }

    private float GetRandomPercent()
    {
        return RandomNumberGeneratorUtils.unityRNG.GetValueInRange(0f, 100f);
    }
}