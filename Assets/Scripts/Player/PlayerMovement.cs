using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //TODO does movement need to be declared up here? structs in C# are declared on the stack
    private Vector3 movement;
    private Animator animator;
    private Rigidbody playerRigidbody;
    private LayerMask lookDirectionMask;
    private float cameraRayLength = 100;

	public void Awake()
    {
        CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
        cameraFollow.target = transform;
        lookDirectionMask = LayerMask.GetMask("MouseRaycast");
        animator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        ActionManager actionManager = Singleton<ActionManager>.Instance;
        actionManager.RegisterMovementListener(Move);
    }
	
    private void Move(float x, float z, float rawX, float rawZ)
    {
        movement.Set(rawX, 0, rawZ);
        if(movement.magnitude > 1)
            movement = movement.normalized;

        movement = movement * PlayerStats.GetCurrentValue(PlayerStats.Stat.MOVEMENT_SPEED) * Time.deltaTime;

        // Apply a force that attempts to reach our target velocity
        movement = (movement - playerRigidbody.velocity);
        //TODO player movement speed is really weird. Why is thi being clamped here like this?
        float maxClamp = PlayerStats.GetMaxValue(PlayerStats.Stat.MOVEMENT_SPEED) * Time.deltaTime;
        movement.x = Mathf.Clamp(movement.x, -1 * maxClamp, maxClamp);
        movement.z = Mathf.Clamp(movement.z, -1 * maxClamp, maxClamp);
        movement.y = 0;
        playerRigidbody.AddForce(movement, ForceMode.VelocityChange);

        Turning();
    }

    private void Turning()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;

        if(Physics.Raycast(cameraRay, out floorHit, cameraRayLength, lookDirectionMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            //transform.LookAt(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);
        }
    }

    private void Animating(float h, float v)
    {
        bool walking = h != 0 || v != 0;
        //TODO use variable hash Id instead of string
        animator.SetBool("IsWalking", walking);
    }
}