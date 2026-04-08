using UnityEngine;

public class OBJ : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 👉 ถ้าเป็น Enemy → ไม่ให้ชน (ให้ทะลุ)
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Collider2D enemyCol = collision.collider;
            Collider2D myCol = GetComponent<Collider2D>();

            Physics2D.IgnoreCollision(enemyCol, myCol);
        }
    }
}
