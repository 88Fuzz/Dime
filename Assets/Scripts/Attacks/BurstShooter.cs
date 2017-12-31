using UnityEngine;

/**
 * Waits a random period of time to fire several bullets in a short burst
 */
public class BurstShooter: MyMonoBehaviour
{
    private delegate void TimerAction();

    public float shortWaitTime;
    public Range longWaitTime;
    public Transform attackPosition;
    public AttackManager attackManager;
    public Range numberOfBulletsInBurst;

    private int maxBulletsToFire;
    private int bulletsFired;
    private float waitTime;
    private float timer;
    private TimerAction timerAction;

    protected override void MyAwake()
    {
        waitTime = 0;
        timer = 0;
        bulletsFired = 0;
        timerAction = FireBulletWithShortWait;
        maxBulletsToFire = (int) RandomNumberGeneratorUtils.unityRNG.GetValueInRange(numberOfBulletsInBurst);
    }

    protected override void MyFixedUpdateWithDeltaTime(float myDeltaTime, float timeScale)
    {
        timer += myDeltaTime;
        if(timer > waitTime)
        {
            timerAction();
            timer = 0;
        }
    }

    private void FireBulletWithShortWait()
    {
        attackManager.StartAttack(attackPosition);
        waitTime = shortWaitTime;
        bulletsFired++;
        if(bulletsFired == maxBulletsToFire - 1)
            timerAction = FireBulletWithLongWait;
    }

    private void FireBulletWithLongWait()
    {
        attackManager.StartAttack(attackPosition);
        waitTime = RandomNumberGeneratorUtils.unityRNG.GetValueInRange(longWaitTime);
        bulletsFired = 0;
        timerAction = FireBulletWithShortWait;
    }
}