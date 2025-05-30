using UnityEngine;

public class WeaponController2 : MonoBehaviour
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

    private bool hasFired = false;

    void Update()
    {
        if (Input.GetAxis("FireP2") == 0)
        {
            hasFired = false;
        }
        if (Input.GetAxis("FireP2") == 1 && pauseMenuUI.activeSelf == false && hasWeapon && !hasFired)
        {
            Shoot();
            hasFired = true;
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
