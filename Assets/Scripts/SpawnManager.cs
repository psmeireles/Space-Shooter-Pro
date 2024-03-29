﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject[] _powerUps;

    [SerializeField]
    private int _spawnEnemyPeriod = 5;

    [SerializeField]
    private GameObject _enemyContainer;

    private bool _stopSpawning = false;

    [SerializeField]
    private GameObject _explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3);
        while (!_stopSpawning)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-10f, 10f), 8f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_spawnEnemyPeriod);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3);
        while (!_stopSpawning)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-10f, 10f), 8f, 0);
            int powerupIndex = Random.Range(0, _powerUps.Length);
            GameObject newPowerup = Instantiate(_powerUps[powerupIndex], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }

    public void OnPlayerDeath(Vector3 playerPosition)
    {
        _stopSpawning = true;
        Instantiate(_explosionPrefab, playerPosition, Quaternion.identity);
    }
}
