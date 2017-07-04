using UnityEngine;

public class Spinner : MyMonoBehaviour
{
    //TODO make this a range
    public float maxSpinRate;
    //TODO make this a range
    public float spinUpRate;
    //TODO make this a range
    public float spinDownRate;
    //TODO make this a range
    public float waitTime;
    //TODO make this a range?
    public float beamStartEndRate;
    public Beam[] beams;

    private int direction;
    private float spinSpeed;
    private float spinChange;
    private float waitTimer;

    protected override void MyAwake()
    {
        spinSpeed = 0;
        spinChange = 0;
        direction = 1;
        StartWait();
    }

    /*
     * Start spinning 
     */
    public void StartSpinUp()
    {
        spinChange = spinUpRate;
        direction = 1;
    }

    /*
     * Stop spinning 
     */
    public void StartSpinDown()
    {
        spinChange = spinDownRate;
    }

    protected override void MyFixedUpdateWithDeltaTime(float myDeltaTime, float timeScale)
    {
        if(waitTimer > 0)
        {
            waitTimer -= myDeltaTime;
            if(waitTimer < 0)
                StartSpinUp();
            return;
        }
        Debug.Log("STARTED SPINNING");

        spinSpeed += spinChange * timeScale;
        Vector3 currentRotation = transform.rotation.eulerAngles;
        currentRotation.y += spinSpeed * direction * myDeltaTime;
        transform.eulerAngles = currentRotation;

        CheckSpinningBounds();
        CheckBeamBounds();
    }

    private void CheckSpinningBounds()
    {
        if(spinSpeed > maxSpinRate)
        {
            spinSpeed = maxSpinRate;
            spinChange = 0;
        }
        else if(spinSpeed < 0)
        {
            spinSpeed = 0;
            spinChange = 0;
            StartWait();
        }
    }

    private void StartWait()
    {
        Debug.Log("STOP SPINNING");
        waitTimer = waitTime;
    }

    private void CheckBeamBounds()
    {
        Test this shit.
        if(!beams[0].IsActive() && spinSpeed > beamStartEndRate)
        {
            Debug.Log("Activate the beam!");
            foreach(Beam beam in beams)
            {
                beam.StartBeam();
            }
        }
        else if(beams[0].IsActive() && spinSpeed < beamStartEndRate)
        {
            Debug.Log("Beam is deactivating");
            foreach(Beam beam in beams)
            {
                beam.EndBeam();
            }
        }
    }
}