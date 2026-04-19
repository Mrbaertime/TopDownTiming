using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Transform player;

    [Header("Chunk Settings")]
    public List<GameObject> chunkPrefabs;
    public float chunkSize = 32f; // ปรับตามขนาดจริงของคุณ
    public int viewDistance = 2;  // 2 = 5x5 chunks

    private Dictionary<Vector2Int, GameObject> spawnedChunks = new Dictionary<Vector2Int, GameObject>();

    void Update()
    {
        GenerateChunks();
    }

    void LateUpdate()
    {
        RemoveFarChunks();
    }

    void GenerateChunks()
    {
        Vector2Int playerChunk = new Vector2Int(
            Mathf.FloorToInt(player.position.x / chunkSize),
            Mathf.FloorToInt(player.position.y / chunkSize)
        );

        for (int x = -viewDistance; x <= viewDistance; x++)
        {
            for (int y = -viewDistance; y <= viewDistance; y++)
            {
                Vector2Int coord = new Vector2Int(playerChunk.x + x, playerChunk.y + y);

                if (!spawnedChunks.ContainsKey(coord))
                {
                    SpawnChunk(coord);
                }
            }
        }
    }

    void SpawnChunk(Vector2Int coord)
    {
        Vector3 spawnPos = new Vector3(coord.x * chunkSize, coord.y * chunkSize, 0);

        int rand = Random.Range(0, chunkPrefabs.Count);
        GameObject chunk = Instantiate(chunkPrefabs[rand], spawnPos, Quaternion.identity);

        spawnedChunks.Add(coord, chunk);
    }

    void RemoveFarChunks()
    {
        Vector2Int playerChunk = new Vector2Int(
            Mathf.FloorToInt(player.position.x / chunkSize),
            Mathf.FloorToInt(player.position.y / chunkSize)
        );

        List<Vector2Int> toRemove = new List<Vector2Int>();

        foreach (var chunk in spawnedChunks)
        {
            Vector2Int coord = chunk.Key;

            int dx = Mathf.Abs(coord.x - playerChunk.x);
            int dy = Mathf.Abs(coord.y - playerChunk.y);

            if (dx > viewDistance || dy > viewDistance)
            {
                Destroy(chunk.Value);
                toRemove.Add(coord);
            }
        }

        foreach (var coord in toRemove)
        {
            spawnedChunks.Remove(coord);
        }
    }
}