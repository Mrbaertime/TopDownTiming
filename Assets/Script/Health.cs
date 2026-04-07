using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Action OnDeath; // 👈 สำคัญมาก

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        OnDeath?.Invoke(); // 👈 เรียก event
    }
}