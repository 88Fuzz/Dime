using UnityEngine;

/*
 * Makes sure the bullet travels in the same direction as when it started.
 */
[CreateAssetMenu(fileName = "StraightVelocity", menuName = "ScriptableObjects/Bullets/BulletVelocity/StraightVelocity")]
public class StraightVelocity : BulletVelocityModifier
{
    public override bool CanBeRemoved()
    {
        return true;
    }

    public override Vector3 ChangeVelocity(Vector3 currentVelocity)
    {
        return currentVelocity;
    }
}