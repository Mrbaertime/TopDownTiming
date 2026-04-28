using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<GameObject> enemyPrefabs;
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
        if (enemyPrefabs.Count == 0 || player == null)
            return;

        // 🎲 สุ่ม enemy
        int rand = Random.Range(0, enemyPrefabs.Count);
        GameObject selectedEnemy = enemyPrefabs[rand];

        // 🎯 สุ่มตำแหน่งรอบ player
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        Vector3 spawnPos = player.position + (Vector3)(randomDir * spawnDistance);

        Instantiate(selectedEnemy, spawnPos, Quaternion.identity);
    }
}