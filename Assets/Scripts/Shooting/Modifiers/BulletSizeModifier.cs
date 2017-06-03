using UnityEngine;

/*
 * Class used to modify the size of the bullet.
 */
public abstract class BulletSizeModifier : ScriptableObject
{
    /*
     * Called every update tick to change the size of the bullet. All bullets are treated as a sphere.
     */
    public abstract float ChangeSize(BasicBullet basicBullet, float deltaTime, float timeScale);

    /*
     * Return true if size changing is done for the bullet.
     */
    public abstract bool IsDoneModifying(BasicBullet basicBullet);

    /*
     * Some bullets can have a bulletSizeModifier that can be overridden by the BulletManager's BulletSizeModifier.
     * Return true if the BulletManager's BulletSizeModifier should be used instead of the current one.
     */
    public abstract bool CanBeRemoved();
}