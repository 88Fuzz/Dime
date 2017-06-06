using UnityEngine;

/*
 * Spawns a basic bullet
 */
[CreateAssetMenu(fileName = "BasicBulletAttackStarter", menuName = "ScriptableObjects/Enemy/AttackManager/BasicBulletAttackStarter")]
public class BasicBulletAttackStarter : AttackManager
{
    public BasicBullet basicBullet;

    public override void EndAttack(MyMonoBehaviour myMonoBehaviour)
    {
        if (myMonoBehaviour)
            myMonoBehaviour.MyDestroy();
    }

    public override MyMonoBehaviour StartAttack(Transform spawnPosition)
    {
        //TODO some kind of object pooling
        return Instantiate(basicBullet, spawnPosition.position, spawnPosition.rotation, null) as MyMonoBehaviour;
    }
}