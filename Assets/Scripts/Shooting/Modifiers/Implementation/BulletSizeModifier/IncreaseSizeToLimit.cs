using UnityEngine;

/*
 * Linearly increases the bullet size to a limit, then the size stays the same.
 */
[CreateAssetMenu(fileName = "IncreaseSizeToLimit", menuName = "ScriptableObjects/Bullets/SizeModifier/IncreaseSizeToLimit")]
public class IncreaseSizeToLimit : BulletSizeModifier
{
    public float maxSize;
    public float changeRate;

    public override bool CanBeRemoved()
    {
        return false;
    }

    public override float ChangeSize(BasicBullet basicBullet, float deltaTime, float timeScale)
    {
        float radius = basicBullet.GetRadius();
        if (radius >= maxSize)
            return radius;

        radius += deltaTime * changeRate;
        if (radius > maxSize)
            radius = maxSize;

        return radius;
    }

    public override bool IsDoneModifying(BasicBullet basicBullet)
    {
        return basicBullet.GetRadius() >= maxSize;
    }
}