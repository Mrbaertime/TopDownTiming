using UnityEngine;

public class PlayerAimShoot : MonoBehaviour
{
    [Header("Aim")]
    private Camera cam;

    [Header("Shoot")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 5f;

    private float fireTimer;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Aim();
        Shoot();
    }

    void Aim()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Shoot()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= 1f / fireRate)
        {
            fireTimer = 0f;
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
}