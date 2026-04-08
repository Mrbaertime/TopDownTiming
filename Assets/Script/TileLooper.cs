using UnityEngine;

public class TileLooper : MonoBehaviour
{
    public Transform player;
    public float tileSize = 10f;

    void Update()
    {
        float offsetX = player.position.x - transform.position.x;
        float offsetY = player.position.y - transform.position.y;

        if (Mathf.Abs(offsetX) > tileSize)
        {
            MoveTile(new Vector3(Mathf.Sign(offsetX) * tileSize * 3, 0, 0));
        }

        if (Mathf.Abs(offsetY) > tileSize)
        {
            MoveTile(new Vector3(0, Mathf.Sign(offsetY) * tileSize * 3, 0));
        }
    }

    void MoveTile(Vector3 move)
    {
        transform.position += move;

        // 👉 รีเซ็ต object ใน tile
        TileSpawner spawner = GetComponent<TileSpawner>();
        if (spawner != null)
        {
            spawner.ResetTile();
        }
    }
}