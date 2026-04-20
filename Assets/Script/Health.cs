using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Action OnDeath; // 👈 เรียกเมื่อตาย
    public Action<int, Vector3> OnTakeDamage; // 👈 เรียกเมื่อโดนดาเมจ (ส่งค่าดาเมจ และจุดที่โดนตีมาด้วย)

    void Start()
    {
        currentHealth = maxHealth;
    }

    // อัปเดตให้รับพิกัด hitSource เพื่อเอาไปทำ Knockback ได้
    public void TakeDamage(int damage, Vector3 hitSource = default)
    {
        currentHealth -= damage;

        // ประกาศ Event ให้ Script อื่น (เช่น Enemy.cs) รู้ว่าโดนตีแล้วนะ
        OnTakeDamage?.Invoke(damage, hitSource);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        OnDeath?.Invoke();
    }
}