using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAimShoot : MonoBehaviour
{
    private Camera cam;

    [Header("References")]
    [SerializeField] private Transform gunPivot;
    [SerializeField] private Transform firePoint;
    [SerializeField] private SpriteRenderer playerSprite;

    [Header("Shoot")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate = 5f;

    private Vector2 aimInput;
    private float fireTimer;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        GetAimInput();
        Aim();
        Shoot();
    }

    void GetAimInput()
    {
        // 🎮 อ่านจาก Gamepad (รวม On-Screen Stick)
        if (Gamepad.current != null)
        {
            Vector2 stick = Gamepad.current.rightStick.ReadValue();

            if (stick.sqrMagnitude > 0.1f)
            {
                aimInput = stick;
                return;
            }
        }

        // 🖱️ fallback → Mouse
        if (Mouse.current != null)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);
            worldPos.z = 0f;

            aimInput = (worldPos - transform.position).normalized;
        }
    }

    void Aim()
    {
        if (aimInput.sqrMagnitude < 0.01f)
            return;

        gunPivot.position = transform.position;

        float angle = Mathf.Atan2(aimInput.y, aimInput.x) * Mathf.Rad2Deg;
        gunPivot.rotation = Quaternion.Euler(0, 0, angle);

        // Flip ตัวละคร
        playerSprite.flipX = aimInput.x > 0;

        // กันปืนกลับหัว
        if (aimInput.x < 0)
            gunPivot.localScale = new Vector3(1, -1, 1);
        else
            gunPivot.localScale = new Vector3(1, 1, 1);
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null)
            return;

        fireTimer += Time.deltaTime;

        if (fireTimer >= 1f / fireRate)
        {
            fireTimer = 0f;
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
}