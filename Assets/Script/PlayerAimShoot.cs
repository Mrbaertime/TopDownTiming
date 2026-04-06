using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAimShoot : MonoBehaviour
{
    private Camera cam;

    [Header("References")]
    [SerializeField] private Transform gunPivot;   // ตัวหมุน
    [SerializeField] private Transform firePoint;  // จุดยิง
    [SerializeField] private SpriteRenderer playerSprite;

    [Header("Shoot")]
    [SerializeField] private GameObject bulletPrefab;
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
        // 📌 ตำแหน่งเมาส์
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);
        worldPos.z = 0f;

        // 📌 ให้ pivot อยู่กลาง player ตลอด
        gunPivot.position = transform.position;

        // 📌 หาทิศ
        Vector2 direction = (worldPos - gunPivot.position).normalized;

        // 📌 หมุนปืน
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gunPivot.rotation = Quaternion.Euler(0, 0, angle);

        // 📌 flip ตัวละคร
        playerSprite.flipX = direction.x > 0;

        // 📌 กันปืนกลับหัว
        if (direction.x < 0)
            gunPivot.localScale = new Vector3(1, -1, 1);
        else
            gunPivot.localScale = new Vector3(1, 1, 1);
    }

    void Shoot()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= 1f / fireRate)
        {
            fireTimer = 0f;

            // 📌 ยิงออกจากปลายปืน
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
}