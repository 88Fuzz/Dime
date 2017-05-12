using System.Collections.Generic;

/*
 * class used to hold PlayerStats like movement speed, damage, health, etc.
 */
public class PlayerStats
{
    public static readonly float VALUE_NOT_FOUND = -1;

    public enum Stat
    {
        MOVEMENT_SPEED,
        HEALTH,
        INVINCIBILITY_COUNT,
        SHOOT_DELAY,
        SHOOT_DAMAGE_MULTIPLIER,
        BULLLET_SPEED,
        BULLET_SIZE,
        CRIT_DAMAGE_MULTIPLIER,
        GLANCE_DAMAGE_MULTIPLIER,
        CRIT_DAMAGE_CHANCE,
        GLANCE_DAMAGE_CHANCE
    };

    private struct StateInformation
    {
        public StateInformation(float maxValue, float minValue, float defaultValue, float change)
        {
            this.maxValue = maxValue;
            this.minValue = minValue;
            this.defaultValue = defaultValue;
            this.change = change;
            this.currentValue = defaultValue;
        }
        public float maxValue;
        public float minValue;
        public float defaultValue;
        public float change;
        public float currentValue;
    };

    private static readonly Dictionary<Stat, StateInformation> STAT_MAP = new Dictionary<Stat, StateInformation>
    {
        //TODO something seems off with these movementSpeed and maxMovementSpeed. Figure it out at some point.
        {Stat.MOVEMENT_SPEED, new StateInformation(2000,400,600,200)},//units per second
        {Stat.HEALTH, new StateInformation(100,0,50,10)},
        {Stat.INVINCIBILITY_COUNT, new StateInformation(10,1,3,1)},
        {Stat.SHOOT_DELAY, new StateInformation(.8f,.15f,.5f,.08f)},//seconds
        {Stat.SHOOT_DAMAGE_MULTIPLIER, new StateInformation(2,.5f,1,.15f)},
        {Stat.BULLLET_SPEED, new StateInformation(40,5,20,5)},//TODO what are the units?
        {Stat.BULLET_SIZE, new StateInformation(2,.05f,.25f,.2f)},//TODO what are the units?
        {Stat.CRIT_DAMAGE_MULTIPLIER, new StateInformation(2.3f,1.7f,2,.15f)},
        {Stat.GLANCE_DAMAGE_MULTIPLIER, new StateInformation(.7f,.2f,.5f,.15f)},
        {Stat.CRIT_DAMAGE_CHANCE, new StateInformation(100,0,2,5)},
        {Stat.GLANCE_DAMAGE_CHANCE, new StateInformation(100,0,5,5)},
    };

    public static float GetMaxValue(Stat stat)
    {
        StateInformation stateInfo;
        if (!STAT_MAP.TryGetValue(stat, out stateInfo))
            return VALUE_NOT_FOUND;

        return stateInfo.maxValue;
    }

    public static float GetMinValue(Stat stat)
    {
        StateInformation stateInfo;
        if (!STAT_MAP.TryGetValue(stat, out stateInfo))
            return VALUE_NOT_FOUND;

        return stateInfo.minValue;
    }

    public static float GetCurrentValue(Stat stat)
    {
        StateInformation stateInfo;
        if (!STAT_MAP.TryGetValue(stat, out stateInfo))
            return VALUE_NOT_FOUND;

        return stateInfo.currentValue;
    }

    public static void IncrementValue(Stat stat)
    {
        StateInformation stateInfo;
        if (!STAT_MAP.TryGetValue(stat, out stateInfo))
            return;

        stateInfo.currentValue += stateInfo.change;
        if (stateInfo.currentValue > stateInfo.maxValue)
            stateInfo.currentValue = stateInfo.maxValue;
    }

    public static void DecrementValue(Stat stat)
    {
        StateInformation stateInfo;
        if (!STAT_MAP.TryGetValue(stat, out stateInfo))
            return;

        stateInfo.currentValue -= stateInfo.change;
        if (stateInfo.currentValue < stateInfo.minValue)
            stateInfo.currentValue = stateInfo.minValue;
    }
}