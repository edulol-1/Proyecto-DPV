using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform playerTrans;
    public float bulletSpeed;
    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && pauseMenuUI.activeSelf == false)
        {
            Shoot1();
        }
    }

    void Shoot1()
    {
        Vector3 direction = playerTrans.forward.normalized;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(direction * bulletSpeed);
        //rb.velocity = direction * bulletSpeed;
    }

    void Shoot()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(1000);
        }

        Vector3 direction = (targetPoint - firePoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = direction * bulletSpeed;
    }
}
