using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform player;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnRate = 2f;
    [SerializeField] private float spawnDistance = 8f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            timer = 0f;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null || player == null)
            return;

        // สุ่มรอบตัว player
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        Vector3 spawnPos = player.position + (Vector3)(randomDir * spawnDistance);

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}