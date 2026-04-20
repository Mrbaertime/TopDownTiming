using UnityEngine;
using System;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Hit Effects")]
    public float knockbackForce = 30f; // ตั้งค่าให้กระเด็นแรงๆ ไว้ก่อน (Unity 6)
    public float knockbackDuration = 0.15f;
    public float hitFlashTime = 0.1f;

    public Action OnDeath;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Color originalColor;

    // 👈 ตัวแปรนี้สำคัญมาก! เอาไว้ให้ Player/Enemy เช็คว่ากำลังกระเด็นอยู่ไหม
    public bool IsKnockedBack { get; private set; } = false;

    void Start()
    {
        currentHealth = maxHealth;

        // ดึง Component อัตโนมัติ (ไม่ต้องลากใส่เอง)
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        // เก็บสีเดิมไว้ก่อน (ป้องกัน Error กรณีไม่มี SpriteRenderer)
        if (sr != null)
        {
            originalColor = sr.color;
        }
    }

    public void TakeDamage(int damage, Vector3 hitSource = default)
    {
        currentHealth -= damage;

        // 🎨 เปลี่ยนสี
        if (sr != null)
        {
            StartCoroutine(HitFlash());
        }

        // 💥 Knockback (เช็คว่ามี Rigidbody และมีทิศทางส่งมาไหม)
        if (rb != null && hitSource != Vector3.zero)
        {
            StartCoroutine(ApplyKnockback(hitSource));
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator HitFlash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(hitFlashTime);
        sr.color = originalColor;
    }

    private IEnumerator ApplyKnockback(Vector3 hitSource)
    {
        IsKnockedBack = true; // ล็อกไม่ให้เดิน

        Vector2 knockDir = (transform.position - hitSource).normalized;
        rb.linearVelocity = Vector2.zero; // ล้างแรงเก่า
        rb.AddForce(knockDir * knockbackForce, ForceMode2D.Impulse); // กระแทก

        yield return new WaitForSeconds(knockbackDuration); // รอจนกว่าจะหายกระเด็น

        IsKnockedBack = false; // ปลดล็อกให้เดินต่อได้
    }

    void Die()
    {
        OnDeath?.Invoke();
    }
}