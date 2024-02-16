using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileController : MonoBehaviour
{
    public float speed = 10f;
    public float rotateSpeed = 200f;
    public int damage = 1;
    public Transform target;
    public GameObject explosionEffect;
    private Rigidbody2D rb;
    private AudioSource audioSource;
    public AudioClip missileSound;



    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform;
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(missileSound);
    }

    private void FixedUpdate() {

            Vector2 direction = (Vector2)target.position - rb.position;

            direction.Normalize();

            float rotationAmount = Vector3.Cross(direction, transform.right).z;

            rb.angularVelocity = -rotationAmount * rotateSpeed;

            rb.velocity = transform.right * speed;

    }


    private void OnCollisionEnter2D(Collision2D other) {
        // // Check if the projectile collides with something
        if (other.gameObject.CompareTag("Player"))
        {
            float remainingHealth = other.gameObject.GetComponent<HealthController>().damageTaken(damage);

            if(remainingHealth > 0){
                other.gameObject.GetComponent<CharacterController>().damageResponse(other.contacts[0].point);
            }
            explode();

            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            explode();
            Destroy(gameObject);
        }
        
        if (other.gameObject.CompareTag("Bullet"))
        {
            explode();
            Destroy(gameObject);
        }
    }

    private void explode()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);
    }
}
