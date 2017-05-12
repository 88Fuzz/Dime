using System.Collections.Generic;
using UnityEngine;

/*
 * Provider called whenever a bullet hits an object to get the damage information the bullet should inflict.
 */
public abstract class BulletHitInformationProvider : ScriptableObject
{
    public enum DamageModifier
    {
        CRIT,
        GLANCE,
        NONE
    };

    //TODO whenever the BulletHitInformationProvider is changed, all the public variables will need to be transferred to the new one.
    public float baseDamage;
    //A LinkList is here because all get/set operations needed are O(1). A queue has a max of O(n) when enqueing.
    public LinkedList<DamageModifier> queuedTypes;

    public void OnEnable()
    {
        queuedTypes = new LinkedList<DamageModifier>();
    }

    public void AddGuarantee(DamageModifier damageModifier)
    {
        AddGuarantee(damageModifier, 1);
    }

    public void AddGuarantee(DamageModifier damageModifier, int count)
    {
        for(int i =0; i < count; i++)
        {
            queuedTypes.AddLast(damageModifier);
        }
    }

    /*
     * If any damage modifiers have been queued up, they will be used here.
     * If none are queued, the GetFallbackHitInformation will be called.
     */
    public float GetHitInformation()
    {
        if (queuedTypes.Count == 0)
            return GetFallbackHitInformation();

        LinkedListNode<DamageModifier> node = queuedTypes.First;
        queuedTypes.RemoveFirst();

        return GetDamageValue(node.Value);
    }

    /*
     * Given the DamageModifier, return the value of damage that should be done.
     */
    protected float GetDamageValue(DamageModifier damageModifier)
    {
        float damage = baseDamage;
        switch (damageModifier)
        {
            case DamageModifier.CRIT:
                damage *= PlayerStats.GetCurrentValue(PlayerStats.Stat.CRIT_DAMAGE_MULTIPLIER);
                break;
            case DamageModifier.GLANCE:
                damage *= PlayerStats.GetCurrentValue(PlayerStats.Stat.GLANCE_DAMAGE_MULTIPLIER);
                break;
            default:
                //Do nothing
                break;
        }

        return damage * PlayerStats.GetCurrentValue(PlayerStats.Stat.SHOOT_DAMAGE_MULTIPLIER);
    }

    /*
     * Return the damage information the bullet should inflict on an object.
     */
    protected abstract float GetFallbackHitInformation();
}