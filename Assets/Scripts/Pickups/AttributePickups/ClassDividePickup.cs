using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * Finds the best two attributes for the player and increases the values.
 * Finds the worst two attributes for the player and decreases the values.
 */
public class ClassDividePickup : Pickup
{
    private static readonly HashSet<PlayerStats.Stat> ATTRIBUTE_TO_IGNORE = new HashSet<PlayerStats.Stat>()
    {
        PlayerStats.Stat.CRIT_DAMAGE_CHANCE,
        PlayerStats.Stat.CRIT_DAMAGE_MULTIPLIER,
        PlayerStats.Stat.GLANCE_DAMAGE_CHANCE,
        PlayerStats.Stat.GLANCE_DAMAGE_MULTIPLIER,
        PlayerStats.Stat.INVINCIBILITY_COUNT
    };

    protected override void PickedUp(GameObject player)
    {
        PlayerStats.Stat[] maxStats = new PlayerStats.Stat[] { PlayerStats.Stat.NONE, PlayerStats.Stat.NONE};
        PlayerStats.Stat[] minStats = new PlayerStats.Stat[] { PlayerStats.Stat.NONE, PlayerStats.Stat.NONE};
        float[] maxValue = new float[] { -1, -1};
        float[] minValue = new float[] { 200, 200 };

        foreach (PlayerStats.Stat stat in Enum.GetValues(typeof(PlayerStats.Stat)))
        {
            if (ATTRIBUTE_TO_IGNORE.Contains(stat))
                continue;

            float percent = PlayerStats.GetPercentOfMax(stat);
            Debug.Log("Stat " + stat + " has value " + percent);
            if (percent == PlayerStats.VALUE_NOT_FOUND)
                continue;

            if(percent > maxValue[0])
            {
                maxValue[1] = maxValue[0];
                maxStats[1] = maxStats[0];
                maxValue[0] = percent;
                maxStats[0] = stat;
            }
            else if(percent > maxValue[1])
            {
                maxValue[1] = percent;
                maxStats[1] = stat;
            }
            else if(percent < minValue[0])
            {
                minValue[1] = minValue[0];
                minStats[1] = minStats[0];
                minValue[0] = percent;
                minStats[0] = stat;
            }
            else if(percent < minValue[1])
            {
                minValue[1] = percent;
                minStats[1] = stat;
            }
        }

        Debug.Log("\nNow chaning attributes\n");
        PlayerStats.IncrementValue(maxStats[0]);
        PlayerStats.IncrementValue(maxStats[1]);
        PlayerStats.DecrementValue(minStats[0]);
        PlayerStats.DecrementValue(minStats[1]);
    }
}