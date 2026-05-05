using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    public int damage = 10;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            col.GetComponent<Health>()?.TakeDamage(damage, transform.position);
        }
    }
}