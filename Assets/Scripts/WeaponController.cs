using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform playerTrans;
    public float bulletSpeed;
    public GameObject pauseMenuUI;
    public bool hasWeapon = false;
    public AudioSource audioSource;
    public AudioClip shootSound;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && pauseMenuUI.activeSelf == false && hasWeapon)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        audioSource.PlayOneShot(shootSound);
        Vector3 direction = playerTrans.forward.normalized;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(direction * bulletSpeed);
    }
}
