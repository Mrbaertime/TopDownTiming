using UnityEngine;

public class ExpPickup : MonoBehaviour
{
    public float expAmount = 5f;

    //public float magnetSpeed = 5f;
    //private Transform player;
    //private bool isMagnetActive = false;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<PlayerLevel>().AddExp(expAmount);
            Destroy(gameObject);
        }
    }

    //void Update()
    //{
    //    if (isMagnetActive && player != null)
    //    {
    //        transform.position = Vector2.MoveTowards(
    //            transform.position,
    //            player.position,
    //            magnetSpeed * Time.deltaTime
    //        );
    //    }
    //}

    //public void ActivateMagnet(Transform target)
    //{
    //    player = target;
    //    isMagnetActive = true;
    //}
}