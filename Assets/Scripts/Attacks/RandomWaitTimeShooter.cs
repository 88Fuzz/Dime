using UnityEngine;

/*
 * Fires a bullet after a random time between waitTimeRange has been waited since the last shot.
 */
public class RandomWaitTimeShooter: MyMonoBehaviour
{
    public Range waitTimeRange;
    public Transform attackPosition;
    public AttackManager attackManager;

    private float waitTime;
    private float timer;

    protected override void MyAwake()
    {
        waitTime = GetWaitTime();
        timer = 0;
    }

    protected override void MyFixedUpdateWithDeltaTime(float myDeltaTime, float timeScale)
    {
        timer += myDeltaTime;

        if(timer > waitTime)
        {
            timer = 0;
            waitTime = GetWaitTime();
            FireBullet();
        }
    }

    //TODO if the base tower is killed while the bullet it "charging" then the bullet still lives.
    private void FireBullet()
    {
        //TODO some kind of object pooling
        attackManager.StartAttack(attackPosition);
    }

    private float GetWaitTime()
    {
        return RandomNumberGeneratorUtils.unityRNG.GetValueInRange(waitTimeRange);
    }
}