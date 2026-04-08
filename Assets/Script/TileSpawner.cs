using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public GameObject[] prefabs;
    public int spawnCount = 5;
    public float tileSize = 10f;

    private bool hasSpawned = false;

    void Start()
    {
        SpawnObjects();
    }

    public void ResetTile()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        hasSpawned = false;
        SpawnObjects();
    }

    void SpawnObjects()
    {
        if (hasSpawned) return;

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 pos = new Vector2(
                transform.position.x + Random.Range(-tileSize / 2, tileSize / 2),
                transform.position.y + Random.Range(-tileSize / 2, tileSize / 2)
            );

            int index = Random.Range(0, prefabs.Length);

            GameObject obj = Instantiate(prefabs[index], pos, Quaternion.identity, transform);

            // Random scale
            float scale = Random.Range(0.8f, 1.2f);
            obj.transform.localScale = Vector3.one * scale;
        }

        hasSpawned = true;
    }
}