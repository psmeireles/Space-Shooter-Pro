using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 20;

    [SerializeField]
    private GameObject _explosionPrefab;

    private SpawnManager _spawnManager;
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager")?.GetComponent<SpawnManager>();
        if (_spawnManager == null)
            Debug.LogError("SpawnManager is null");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            GameObject explosion = Instantiate(_explosionPrefab, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.25f);
            _spawnManager.StartSpawning();
            Destroy(other.gameObject);
        }
    }
}
