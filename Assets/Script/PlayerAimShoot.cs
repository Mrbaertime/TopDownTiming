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

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSFX;

    private Vector2 aimInput;
    private float fireTimer;

    public bool isDoubleShot = false;
    public bool isTripleShot = false;

    public float spreadAngle = 10f;

    public float bulletScale = 1f;

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
        if (Gamepad.current != null)
        {
            Vector2 stick = Gamepad.current.rightStick.ReadValue();

            if (stick.sqrMagnitude > 0.1f)
            {
                aimInput = stick;
                return;
            }
        }

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

        playerSprite.flipX = aimInput.x > 0;

        if (aimInput.x < 0)
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

            if (shootSFX != null)
            {
                audioSource.PlayOneShot(shootSFX);
            }

            if (isTripleShot)
            {
                Quaternion left = firePoint.rotation * Quaternion.Euler(0, 0, -spreadAngle);
                Quaternion mid = firePoint.rotation;
                Quaternion right = firePoint.rotation * Quaternion.Euler(0, 0, spreadAngle);

                SpawnBullet(left);
                SpawnBullet(mid);
                SpawnBullet(right);
            }
            else if (isDoubleShot)
            {
                Quaternion left = firePoint.rotation * Quaternion.Euler(0, 0, -spreadAngle);
                Quaternion right = firePoint.rotation * Quaternion.Euler(0, 0, spreadAngle);

                SpawnBullet(left);
                SpawnBullet(right);
            }
            else
            {
                SpawnBullet(firePoint.rotation);
            }
        }
    }

    // 🔥 ฟังก์ชันรวม ยิง + scale
    void SpawnBullet(Quaternion rot)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rot);
        bullet.transform.localScale *= bulletScale;
    }
}