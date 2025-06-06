using UnityEngine;

public class Bullet2 : MonoBehaviour
{
    public float lifeSpan = 2.0f;
    public float speed = 10f;
    public int damageAmount = 20;
    
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)

        rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);
        
        Destroy(gameObject, lifeSpan);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
            Destroy(gameObject);
        }
    }
}