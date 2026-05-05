using UnityEngine;

public class MagnetRuntime : MonoBehaviour
{
    public float radius = 5f;
    public float pullSpeed = 10f;

    void Update()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Exp"))
            {
                ExpPickup exp = hit.GetComponent<ExpPickup>();
                if (exp != null)
                {
                    exp.ActivateMagnet(transform, pullSpeed);
                }
            }
        }
    }

    // Debug ǧ�ٴ
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}