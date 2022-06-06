using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject [] _powerUpPrefabs;
    [SerializeField] private float _enemySpawnTime=3.0f;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private float _powerUpSpawnSeed=20f;
    [SerializeField] GameObject _asteroidPrefab;
    private bool _stopSpawn=false;


    void Start()
    {
        GameObject asteroid = Instantiate(_asteroidPrefab, new Vector3(0,3.2f,0), Quaternion.identity);
        asteroid.transform.parent = gameObject.transform;
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2f);
        while(!_stopSpawn)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-9.3f,9.3f),6.3f,0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_enemySpawnTime);
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(2f);
        while(!_stopSpawn)
        {
            int seed=Random.Range(0,3);
            Instantiate(_powerUpPrefabs[seed], new  Vector3(Random.Range(-9.3f,9.3f),6.3f,0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(_powerUpSpawnSeed-7,_powerUpSpawnSeed+7));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawn=true;
        foreach(Transform enemy in transform.GetChild(0).transform)
        {
            Destroy(enemy.gameObject, 2.3f);
        }
    }

    public void StartGame()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }
}
