using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    public int damage = 10;
    private Transform player;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Health healthComponent;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        healthComponent = GetComponent<Health>();

        // สมัครรับ Event ตอนตาย
        healthComponent.OnDeath += OnEnemyDeath;
    }

    void Update()
    {
        if (player == null) return;

        // ⛔ เช็คจาก Health ว่ากำลังโดน Knockback อยู่ไหม? ถ้าโดนอยู่ให้หยุดทำงานไปเลย
        if (healthComponent.IsKnockedBack)
        {
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
        // 💥 ชน Player -> ทำดาเมจใส่ Player
        if (collision.gameObject.CompareTag("Player"))
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage, transform.position);
            }
        }
    }

    void OnEnemyDeath()
    {
        StartCoroutine(DieSequence());
    }

    IEnumerator DieSequence()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}