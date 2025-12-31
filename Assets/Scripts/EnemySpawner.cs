using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Префабы врагов")]
    public GameObject[] enemyPrefabs; // Массив из 3 префабов

    [Header("Настройки спавна")]
    public Transform player; // Ссылка на игрока
    public float spawnRadius = 10f; // Радиус вокруг игрока для спавна

    [Header("Тайминги спавна")]
    public float phase1Duration = 300f;    // 5 минут
    public float phase2Duration = 600f;    // 10 минут (с 5 до 15)

    private Coroutine spawnRoutine;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player не назначен в EnemySpawner!");
            return;
        }

        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogError("Нет префабов врагов!");
            return;
        }

        spawnRoutine = StartCoroutine(SpawnEnemiesOverTime());
    }

    IEnumerator SpawnEnemiesOverTime()
    {
        while (true)
        {
            float gameTime = Time.time;

            int count;
            float interval;

            if (gameTime < phase1Duration)
            {
                count = 4;
                interval = 4f;
            }
            else if (gameTime < phase1Duration + phase2Duration)
            {
                count = 8;
                interval = 3f;
            }
            else
            {
                count = 12;
                interval = 2.5f;
            }

            // Спавним указанное количество врагов
            for (int i = 0; i < count; i++)
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(interval);
        }
    }

    void SpawnEnemy()
    {
        // Выбираем случайный префаб
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        // Генерируем позицию вокруг игрока
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = player.position + new Vector3(randomCircle.x, 0f, randomCircle.y);

        // Спавним врага
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}