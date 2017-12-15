using UnityEngine;

public class Charger : MyMonoBehaviour
{
    public float rotationSpeed;
    public float chargeSpeed;
    public LayerMask wallMask;
    public float lookAheadWallDistance;
    //Player mask should also contain walls so that it does not try to charge through walls/over holes
    public LayerMask playerMask;
    public float lookAheadPlayerDistance;
    public float coolDownAfterHit;
    public float hitBackForce;
    public float hitBackForceVertical;

    private float rechargeTimer;
    private FixedUpdateAction updateAction;
    private ForceManager forceManager;
    private Rigidbody rb;

    protected override void MyAwake()
    {
        rechargeTimer = 0;
        updateAction = RotateAction;
        forceManager = GetComponent<ForceManager>();
        rb = GetComponent<Rigidbody>();
    }

    protected override void MyFixedUpdateWithDeltaTime(float myDeltaTime, float timeScale)
    {
        updateAction(myDeltaTime, timeScale);
    }

    private void RotateAction(float deltaTime, float timeScale)
    {
        transform.Rotate(Vector2.up * (rotationSpeed * deltaTime));

        RaycastHit hitInfo;
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, lookAheadPlayerDistance, playerMask))
        {
            Player player = hitInfo.collider.GetComponent<Player>();
            if(player == null)
                return;

            updateAction = ChargeAction;
        }
    }

    private void ChargeAction(float deltaTime, float timeScale)
    {
        RaycastHit hitInfo;
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, lookAheadWallDistance, wallMask))
        {
            // TODO also add force if the player is hit??
            // Add as way to add forces to objects that are supposed to be slowed down by the affect of time
            forceManager.AddForce(transform.forward * -hitBackForce, coolDownAfterHit/2);
            updateAction = RechargeWait;

            return;
        }
        rb.position = PhysicsUtils.GetNewPosition(transform.position, transform.forward * chargeSpeed, deltaTime);
    }

    private void RechargeWait(float deltaTime, float timeScale)
    {
        rechargeTimer += deltaTime;
        if(rechargeTimer >= coolDownAfterHit)
        {
            rechargeTimer = 0;
            updateAction = RotateAction;
        }
    }
}