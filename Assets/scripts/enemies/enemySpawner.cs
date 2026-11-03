using UnityEngine;
using System.Collections;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform leftSpawnPoint;
    [SerializeField] private Transform rightSpawnPoint;
    [SerializeField] private TMP_Text waveText;

    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float timeBetweenWaves = 4f;
    [SerializeField] private GameObject flyingEnemyPrefab;
    [SerializeField] private float flyingSpawnChance = 0.3f;
    [SerializeField] private GameObject runnerEnemyPrefab;
    [SerializeField] private GameObject tankEnemyPrefab;
    [SerializeField] private int maxGroupSize = 3;
    [SerializeField] private float minSpawnDelay = 0.4f;
    [SerializeField] private float groupSpacing = 0.7f;

    private int waveNumber = 0;
    private int enemiesRemaining = 0;
    private Camera mainCamera;

    private void Start()
    {
        StartCoroutine(StartNextWave());
    }

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private IEnumerator StartNextWave()
    {
        waveNumber++;
        waveText.text = "wave: " + waveNumber;

        int enemyCount = 5 + (waveNumber - 1) * 3;
        enemiesRemaining = enemyCount;

        Debug.Log("Wave " + waveNumber + " started!");

        int spawned = 0;

        // Wave 1 uses a single side for a simpler introduction.
        bool tutorialLeft = Random.value < 0.5f;

        while (spawned < enemyCount)
        {
            int groupSize = 1;

            if (waveNumber > 1)
            {
                int maxSize = Mathf.Min(
                    maxGroupSize,
                    enemyCount - spawned
                );

                groupSize = Random.Range(1, maxSize + 1);
            }

            int leftCount = 0;
            int rightCount = 0;
            for (int i = 0; i < groupSize; i++)
            {
                bool spawnLeft;

                if (waveNumber == 1)
                {
                    spawnLeft = tutorialLeft;
                }
                else
                {
                    spawnLeft = Random.value < 0.5f;
                }

                float offset;

                if (spawnLeft)
                {
                    offset = leftCount * groupSpacing;
                    leftCount++;
                }
                else
                {
                    offset = rightCount * groupSpacing;
                    rightCount++;
                }

                SpawnEnemyOnSide(spawnLeft, offset);
                spawned++;
            }

            float maxDelay = Mathf.Max(
                minSpawnDelay,
                spawnInterval - (waveNumber - 1) * 0.15f
            );

            float delay = Random.Range(minSpawnDelay, maxDelay);

            yield return new WaitForSeconds(delay);
        }        

        while (enemiesRemaining > 0)
        {
            yield return null;
        }

        Debug.Log("Wave " + waveNumber + " complete!");

        yield return new WaitForSeconds(timeBetweenWaves);

        StartCoroutine(StartNextWave());
    }

    private Vector3 GetSpawnPosition(bool spawnLeft, bool isFlying)
    {
        float cameraHeight = mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        float x = spawnLeft
            ? mainCamera.transform.position.x - cameraWidth - 1f
            : mainCamera.transform.position.x + cameraWidth + 1f;

        float y = 0f;

        if (isFlying)
        {
            float bottom = mainCamera.transform.position.y - cameraHeight;
            float top = mainCamera.transform.position.y + cameraHeight;

            y = Random.Range(
                Mathf.Max(2f, bottom + 1f),
                top - 1f
            );
        }

        return new Vector3(x, y, 0f);
    }

    private void SpawnEnemy()
    {
        SpawnEnemyOnSide(Random.value < 0.5f);
    }

    private void SpawnEnemyAt(Transform spawnPoint)
    {
        SpawnEnemyOnSide(spawnPoint == leftSpawnPoint);
    }

    private void SpawnEnemyOnSide(bool spawnLeft, float offset = 0f)
    {
        GameObject prefab = enemyPrefab;
        bool isFlying = false;

        int availableTypes = Mathf.Min(waveNumber, 4);
        int enemyType = Random.Range(0, availableTypes);

        if (enemyType == 1 && runnerEnemyPrefab != null)
        {
            prefab = runnerEnemyPrefab;
        }
        else if (enemyType == 2 && flyingEnemyPrefab != null)
        {
            prefab = flyingEnemyPrefab;
            isFlying = true;
        }
        else if (enemyType == 3 && tankEnemyPrefab != null)
        {
            prefab = tankEnemyPrefab;
        }

        Vector3 position = GetSpawnPosition(spawnLeft, isFlying);
        // Spread enemies farther outside the screen.
        if (spawnLeft)
        {
            position.x -= offset;
        }
        else
        {
            position.x += offset;
        }

        Instantiate(prefab, position, Quaternion.identity);
    }

    public void EnemyRemoved()
    {
        enemiesRemaining--;
    }
}