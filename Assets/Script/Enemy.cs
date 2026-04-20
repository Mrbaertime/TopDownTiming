using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))] // 👈 บังคับให้ต้องมี Health.cs แนบอยู่ด้วยเสมอ
public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    public int damage = 10;
    private Transform player;

    [Header("Hit Effect")]
    public float knockbackForce = 2f;
    public float hitFlashTime = 0.1f;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Color originalColor;
    private Health healthComponent; // 👈 ตัวแปรอ้างอิง Health

    private float knockbackTimer = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        originalColor = sr.color;

        // ดึงคอมโพเนนต์ Health และสมัครรับ Event
        healthComponent = GetComponent<Health>();
        healthComponent.OnDeath += OnEnemyDeath;
        healthComponent.OnTakeDamage += HandleDamageEffects; // 👈 เมื่อ Health โดนดาเมจ ให้ทำเอฟเฟกต์
    }

    void Update()
    {
        if (player == null) return;

        // ⛔ ถ้ายังโดน knockback อยู่ → ไม่ต้องเดิน
        if (knockbackTimer > 0)
        {
            knockbackTimer -= Time.deltaTime;
            return;
        }

        Vector2 direction = (player.position - transform.position).normalized;

        // 🎯 Flip ตาม player
        if (Mathf.Abs(direction.x) > 0.05f)
        {
            sr.flipX = direction.x > 0;
        }

        rb.linearVelocity = direction * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 💥 ชน Player -> ทำดาเมจใส่ Player ปกติ
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(damage, transform.position);
        }
    }

    // ฟังก์ชันนี้จะถูกเรียกอัตโนมัติเมื่อ Health.cs โดนสั่ง TakeDamage
    void HandleDamageEffects(int damage, Vector3 hitSource)
    {
        StartCoroutine(HitFlash());

        if (hitSource != Vector3.zero)
        {
            Vector2 knockDir = (transform.position - hitSource).normalized;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(knockDir * knockbackForce, ForceMode2D.Impulse);
            knockbackTimer = 0.15f;

            // 👈 เพิ่มบรรทัดนี้เพื่อเช็คว่ามันคำนวณแรงถูกไหม
            Debug.Log($"โดนตี! ทิศทางกระเด็น: {knockDir} | แรงที่ใช้: {knockbackForce}");
        }
        else
        {
            Debug.LogWarning("ไม่มีจุด Hit Source ส่งมา ทำ Knockback ไม่ได้!");
        }
    }

    IEnumerator HitFlash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(hitFlashTime);
        sr.color = originalColor;
    }

    void OnEnemyDeath()
    {
        // พอตายก็เรียก Coroutine ตัวแดงก่อนทำลายทิ้ง
        StartCoroutine(DieSequence());
    }

    IEnumerator DieSequence()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}