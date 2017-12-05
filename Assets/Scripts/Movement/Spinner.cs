using UnityEngine;

public class Spinner : MyMonoBehaviour
{
    private delegate void WaitEndTrigger();
    public enum SpinDirection
    {
        CLOCKWISE,
        COUNTER_CLOCKWISE
    };

    //TODO make this a range
    public float maxSpinRate;
    //TODO make this a range
    public float spinUpRate;
    //TODO make this a range
    public float spinDownRate;
    //TODO make this a range
    public float idleWaitTime;
    //TODO make this a range
    public float beamWaitTime;
    //TODO make this a range?
    public float beamStartEndRate;
    public SpinDirection spinDirection;
    public Beam[] beams;

    private float spinSpeed;
    private float spinChange;
    private float waitTimer;
    private FixedUpdateAction fixedUpdateAction;
    private WaitEndTrigger waitEndTrigger;

    protected override void MyAwake()
    {
        if(spinDownRate > 0)
            spinDownRate = -spinDownRate;

        spinSpeed = 0;
        spinChange = 0;
        fixedUpdateAction = DoNothing;
        waitEndTrigger = NoTrigger;
        SetUpIdleWait();
    }

    protected override void MyFixedUpdateWithDeltaTime(float myDeltaTime, float timeScale)
    {
        fixedUpdateAction(myDeltaTime, timeScale);
    }

    private void Spin(float myDeltaTime, float timeScale)
    {
        spinSpeed += spinChange * timeScale;
        Vector3 currentRotation = transform.rotation.eulerAngles;
        currentRotation.y += spinSpeed * GetSpinDirection() * myDeltaTime;
        transform.eulerAngles = currentRotation;
    }

    private void WaitForBeamToBeFullForce(float myDeltaTime, float timeScale)
    {
        bool beamsAtMax = true;
        foreach(Beam beam in beams)
        {
            if (!beam.AtMaxBeamWidth())
            {
                beamsAtMax = false;
                break;
            }
        }

        if (beamsAtMax)
            SetUpSpinUp();
    }

    private void StartBeamFire()
    {
        fixedUpdateAction = WaitForBeamToBeFullForce;
        foreach(Beam beam in beams)
        {
            beam.StartBeam();
        }
    }

    private void WaitForBeamToBeZeroForce(float myDeltaTime, float timeScale)
    {
        bool beamsAtMin = true;
        foreach (Beam beam in beams)
        {
            if (!beam.AtMinBeamWidth())
            {
                beamsAtMin = false;
                break;
            }
        }

        if (beamsAtMin)
            SetUpIdleWait();
    }

    private void EndBeamFire()
    {
        fixedUpdateAction = WaitForBeamToBeZeroForce;
        foreach (Beam beam in beams)
        {
            beam.EndBeam();
        }
    }

    private void WaitAction(float myDeltaTime, float timeScale)
    {
        waitTimer -= myDeltaTime;
        if (waitTimer < 0)
            waitEndTrigger();
    }

    private void ConstantSpin(float myDeltaTime, float timeScale)
    {
        Spin(myDeltaTime, timeScale);
        WaitAction(myDeltaTime, timeScale);
    }

    private void SetUpConstantSpin()
    {
        waitTimer = beamWaitTime;
        fixedUpdateAction = ConstantSpin;
        waitEndTrigger = SetUpSpinDown;
    }

    private void SpinUp(float myDeltaTime, float timeScale)
    {
        Spin(myDeltaTime, timeScale);

        if(spinSpeed > maxSpinRate)
        {
            spinSpeed = maxSpinRate;
            spinChange = 0;
            SetUpConstantSpin();
        }
    }

    private void SetUpSpinUp()
    {
        spinChange = spinUpRate;
        fixedUpdateAction = SpinUp;
    }

    private void SpinDown(float myDeltaTime, float timeScale)
    {
        Spin(myDeltaTime, timeScale);

        if (spinSpeed < 0)
        {
            spinSpeed = 0;
            spinChange = 0;
            EndBeamFire();
        }
    }

    private void SetUpSpinDown()
    {
        spinChange = spinDownRate;
        fixedUpdateAction = SpinDown;
    }

    private void SetUpIdleWait()
    {
        waitTimer = idleWaitTime;
        fixedUpdateAction = WaitAction;
        waitEndTrigger = StartBeamFire;
    }

    private int GetSpinDirection()
    {
        if (spinDirection == SpinDirection.COUNTER_CLOCKWISE)
            return -1;

        return 1;
    }

    private void DoNothing(float myDeltaTime, float timeScale) { }
    private void NoTrigger() { }
}