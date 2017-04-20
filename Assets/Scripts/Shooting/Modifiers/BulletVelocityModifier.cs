using UnityEngine;

/*
 * Class used to modify the velocity of the bullet.
 */
public abstract class BulletVelocityModifier : ScriptableObject
{
    /*
     * Called every update tick to change the velocity of the bullet.
     * TODO, should this be global or local velocity?? Right now I am leaning towards global velocity
     */
    public abstract Vector3 ChangeVelocity(Vector3 currentVelocity);

    /*
     * Some bullets can have a bulletVelocity that can be overridden by the BulletManager's BulletVelocityModifier.
     * Return true if the BulletManager's BulletVelocityModifier should be used instead of the current one.
     */
    public abstract bool CanBeRemoved();
}