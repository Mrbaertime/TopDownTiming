using UnityEngine;

public class ExpPickup : MonoBehaviour
{
    public float expAmount = 5f;

    private Transform target;
    private float speed;
    private bool isMagnet = false;

    void Update()
    {
        if (isMagnet && target != null)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                target.position,
                speed * Time.deltaTime
            );
        }
    }

    public void ActivateMagnet(Transform player, float pullSpeed)
    {
        target = player;
        speed = pullSpeed;
        isMagnet = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<PlayerLevel>().AddExp(expAmount);
            Destroy(gameObject);
        }
    }
}