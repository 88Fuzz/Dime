using UnityEngine;

//I think this can be deleted. I believe it's from a tutorial
public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = .5f;
    public int attackDamage = 10;

    private Animator animator;
    private GameObject target;
    private bool playerInRange;
    private float timer;

	void Awake()
    {
        //TODO have a better way of finding a target
        target = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();

        timer = 0;
	}

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == target)
            playerInRange = true;
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject == target)
            playerInRange = false;
    }
	
	public void FixedUpdate()
    {
        timer += Time.deltaTime;
	
        if(timer >= timeBetweenAttacks && playerInRange)
            Attack();
	}

    private void Attack()
    {
        timer = 0;
        Hittable hittable = target.GetComponent<Hittable>();
        if (hittable)
            hittable.Hit(10);
    }
}