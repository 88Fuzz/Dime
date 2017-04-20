using UnityEngine;

public class SimpleBullet : Bullet
{
    public float baseDamage;

    private int layerMask;

    protected override void Destroyed()
    {
        Destroy(gameObject);
    }

    protected override float GetHitInformation()
    {
        return baseDamage * PlayerStats.shootDamageMultiplier;
    }

    protected override int GetLayerMask()
    {
        return layerMask;
    }

    protected override void Initalize()
    {
        SetRadius(PlayerStats.shootSize);
        layerMask = LayerMask.GetMask("Enemy");
    }

    public override Vector3 GetInitialVelocity()
    {
        return Vector3.forward * PlayerStats.shootSpeed;
    }
}