using UnityEngine;

//I think this can be deleted from the tutorial.
public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    public float speed = 8;

    private Rigidbody enemyRigidBody;

	public void Start()
    {
        transform.LookAt(target);
        enemyRigidBody = GetComponent<Rigidbody>();
	}
	
    //TODO smoothing everything here.
	public void FixedUpdate()
    {
        transform.LookAt(target);
        Vector3 moveDirection = target.transform.position - transform.position;
        moveDirection = moveDirection.normalized * speed * Time.deltaTime;
        enemyRigidBody.MovePosition(transform.position + moveDirection);
	}
}