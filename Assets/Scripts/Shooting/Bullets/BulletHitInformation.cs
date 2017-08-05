using UnityEngine;

public struct BulletHitInformation
{
    public BulletHitInformation(ParticleSystem particleSystem, float damage)
    {
        _particleSystem = particleSystem;
        _damage = damage;
    }

    private float _damage;
    private ParticleSystem _particleSystem;

    public float Damage
    {
        get { return _damage; }
    }

    public ParticleSystem ParticleSystem
    {
        get { return _particleSystem; }
    }
}