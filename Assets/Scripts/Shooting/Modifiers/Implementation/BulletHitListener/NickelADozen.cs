using UnityEngine;

/*
 * For every 12 enemies killed the player will get a garenteed 5 crits for the next 5 shots.
 */
[CreateAssetMenu(fileName = "NickelADozen", menuName = "ScriptableObjects/Bullets/BulletHitListener/NickelADozen")]
public class NickelADozen : BulletHitListener
{
    public static readonly int KILL_LIMIT = 12;
    public static readonly int CRIT_BONUS = 5;

    private int killCount = 0;

    public override bool OnEnemyHit(Bullet bullet, Hittable enemy)
    {
        //Do nothing
        return true;
    }

    public override bool OnEnemyKill(Bullet bullet, Hittable enemy)
    {
        if(++killCount >= KILL_LIMIT)
        {
            //TODO should there be a sound or visual note that the effect was triggered?
            killCount = 0;
            bullet.bulletManager.hitInformationProvider.AddGuarantee(BulletHitInformationProvider.DamageModifier.CRIT, CRIT_BONUS);
        }
        return true;
    }

    public override void OnObjectHit(Bullet bullet, GameObject obj)
    {
        //Do nothing
    }
}