﻿using UnityEngine;

/*
 * Start with a base damage and apply the PlayerStats multiplyer to the value.
 */
[CreateAssetMenu(fileName = "DefaultBulletHitInformationProvider", menuName = "ScriptableObjects/Bullets/BulletHitInformationProvider/DefaultBulletHitInformationProvider")]
public class DefaultBulletHitInformationProvider : BulletHitInformationProvider
{
    protected override DamageModifier GetFallbackDamageModifier()
    {
        float random = RandomNumberGeneratorUtils.unityRNG.GetValueInRange(0f, 200f);

        if (random < PlayerStats.GetCurrentValue(PlayerStats.Stat.CRIT_DAMAGE_CHANCE))
            return DamageModifier.CRIT;

        random -= 100;
        if (random >= 0 && random < PlayerStats.GetCurrentValue(PlayerStats.Stat.GLANCE_DAMAGE_CHANCE))
            return DamageModifier.GLANCE;

        return DamageModifier.NONE;
    }
}