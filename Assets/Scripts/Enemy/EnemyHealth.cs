using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 50;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public AudioClip deathClip;

    private int currentHealth;
    private Animator animator;
    private AudioSource enemyAudio;
    private ParticleSystem hitParticles;
    private CapsuleCollider capsuleCollider;
    private bool isDead;
    private bool isSinking;


	public void Awake()
    {
        animator = GetComponent<Animator>();
        enemyAudio = GetComponent<AudioSource>();
        hitParticles = GetComponentInChildren<ParticleSystem>();
        capsuleCollider = GetComponent<CapsuleCollider>();
	}
	
	public void Update()
    {
        if(isSinking)
        {
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
        }
	}

    public void TakeDamage(int amount, Vector3 hitPoint)
    {
        if (isDead)
            return;

        //enemyAudio.Play();
        //hitParticles.transform.position = hitPoint;
        //hitParticles.Play();
        currentHealth -= amount;

        if(currentHealth <=0)
        {
            Death();
        }
    }

    public bool IsDead()
    {
        return isDead;
    }

    private void Death()
    {
        isDead = true;
        capsuleCollider.isTrigger = true;
        //animator.SetTrigger("Dead");
        //enemyAudio.clip = deathClip;
        //enemyAudio.Play();
    }

    //This method should be called by an animation event!
    public void StartSinking()
    {
        //GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        isSinking = true;
        Destroy(gameObject, 3f);
    }
}