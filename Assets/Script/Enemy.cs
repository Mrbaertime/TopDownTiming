using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    private Transform player;

    [Header("Health")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Hit Effect")]
    public float knockbackForce = 2f;
    public float hitFlashTime = 0.1f;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Color originalColor;

    private float knockbackTimer = 0f; // 👈 กัน movement ทับแรง

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        originalColor = sr.color;
        currentHealth = maxHealth;

        GetComponent<Health>().OnDeath += OnEnemyDeath;
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
        // 💥 ชน Player
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(10);
        }

        // 🔫 โดนกระสุน
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(1, collision.transform.position);
            Destroy(collision.gameObject);
        }
    }

    void TakeDamage(int damage, Vector3 hitSource)
    {
        currentHealth -= damage;

        // 🎨 เปลี่ยนสี
        StartCoroutine(HitFlash());

        // 💥 Knockback
        Vector2 knockDir = (transform.position - hitSource).normalized;
        rb.linearVelocity = Vector2.zero; // 👈 ล้างแรงเก่า
        rb.AddForce(knockDir * knockbackForce, ForceMode2D.Impulse);

        knockbackTimer = 0.15f; // 👈 หยุดเดินแป๊บนึง

        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator HitFlash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(hitFlashTime);
        sr.color = originalColor;
    }

    IEnumerator Die()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }

    void OnEnemyDeath()
    {
        Destroy(gameObject);
    }

}