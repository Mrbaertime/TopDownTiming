using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))] // 👈 บังคับว่าต้องมี Health.cs แนบอยู่ด้วย
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private InputSystem_Actions inputActions;
    private Health health; // 👈 สร้างตัวแปรมาเก็บ Health

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>(); // 👈 ดึง Component แค่ครั้งเดียวตอนเริ่มเกม
        inputActions = new InputSystem_Actions();
    }

    //เสริมมาก่อน เอามาใช้ก่อน
    void Start()
    {
        health.OnDeath += OnPlayerDeath;
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Update()
    {
        // อ่านค่า input เก็บไว้เฉยๆ ไม่ต้องดักอะไรตรงนี้
        moveInput = inputActions.Player.Move.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        // ⛔ ย้ายมาดักตรงนี้แทน! ถ้ากำลังกระเด็นอยู่ ให้ return ออกไปเลย ห้ามเซ็ตค่า linearVelocity
        if (health.IsKnockedBack)
        {
            return;
        }

        // เคลื่อนที่ตามปกติ (โค้ดบรรทัดนี้จะไม่ทำงานถ้าติด return ด้านบน)
        rb.linearVelocity = moveInput * moveSpeed;
    }

    //เสริมมาก่อน เอามาใช้ก่อน
    void OnPlayerDeath()
    {
        GameManager.Instance.GameOver();
    }
}