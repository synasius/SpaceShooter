using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private float _enemySpawnInterval = 5f;

    [SerializeField]
    private GameObject[] _powerUps;

    [SerializeField]
    private float _powerUpSpawnIntervalMin = 5f;

    [SerializeField]
    private float _powerUpSpawnIntervalMax = 15f;

    private bool _stopSpawning = false;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnPowerUps());
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(_enemySpawnInterval);

        while (!_stopSpawning)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-10f, 10f), 11f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_enemySpawnInterval);
        }
    }
    IEnumerator SpawnPowerUps()
    {
        // This yield is needed to avoid spawning an object at the beginning of
        // game. It must be out of the while loop, otherwise the object
        // would be spawned if the player dies before the loop begins.
        yield return new WaitForSeconds(Random.Range(_powerUpSpawnIntervalMin, _powerUpSpawnIntervalMax));

        while (!_stopSpawning)
        {
            float spawnInterval = Random.Range(_powerUpSpawnIntervalMin, _powerUpSpawnIntervalMax);
            Vector3 spawnPosition = new Vector3(Random.Range(-10f, 10f), 11f, 0);
            int randomIndex = Random.Range(0, _powerUps.Length);
            Instantiate(_powerUps[randomIndex], spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
