using UnityEngine;

public class Bullet : MonoBehaviour
{
    // public float velocity;
    // public Rigidbody rb;
    public float lifeSpan = 2.0f;
    public Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        // rb.AddForce(transform.forward * velocity);
        Destroy(this.gameObject, lifeSpan);
    }
}
