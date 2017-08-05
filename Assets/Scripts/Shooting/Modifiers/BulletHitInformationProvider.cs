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
    public ParticleSystem critParticles;
    public ParticleSystem normalParticles;
    public ParticleSystem glanceParticles;

    //A LinkList is here because all get/set operations needed are O(1). A queue has a max of O(n) when enqueing.
    private LinkedList<DamageModifier> queuedTypes;

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
    public BulletHitInformation GetHitInformation()
    {
        DamageModifier damageModifier = DamageModifier.NONE;

        if (queuedTypes.Count == 0)
        {
            damageModifier = GetFallbackDamageModifier();
        }
        else
        {
            LinkedListNode<DamageModifier> node = queuedTypes.First;
            queuedTypes.RemoveFirst();
            damageModifier = node.Value;
        }

        return CreateBulletHitInformation(damageModifier);
    }

    /*
     * Creates a BulletHitInformation based on the DamageModifier.
     */
    protected BulletHitInformation CreateBulletHitInformation(DamageModifier damageModifier)
    {
        return new BulletHitInformation(GetParticleSystem(damageModifier), GetDamageValue(damageModifier));
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
     * Given the DamageModifier, return the particle system that should be used when hitting an enemy.
     */
    protected ParticleSystem GetParticleSystem(DamageModifier damageModifier)
    {
        switch(damageModifier)
        {
            case DamageModifier.CRIT:
                return critParticles;
            case DamageModifier.GLANCE:
                return glanceParticles;
            case DamageModifier.NONE:
                return normalParticles;
            default:
                return normalParticles;
        }
    }

    /*
     * Return the damage information the bullet should inflict on an object.
     */
    protected abstract DamageModifier GetFallbackDamageModifier();
}