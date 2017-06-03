using UnityEngine;

/*
 * Always returns the current size of the bullet, i.e. the bullet's size will never change
 */
[CreateAssetMenu(fileName = "StaySameSize", menuName = "ScriptableObjects/Bullets/SizeModifier/StaySameSize")]
public class StaySameSize : BulletSizeModifier
{
    public override bool CanBeRemoved()
    {
        return true;
    }

    public override float ChangeSize(BasicBullet basicBullet, float deltaTime, float timeScale)
    {
        return basicBullet.GetRadius();
    }

    public override bool IsDoneModifying(BasicBullet basicBullet)
    {
        return false;
    }
}