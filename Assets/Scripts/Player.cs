using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 1f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private float _fireRate = 0.5f;

    private float _canFire = -1f;

    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;

    [SerializeField]
    private bool _isTripleShotActive = false;

    [SerializeField]
    private bool _isSpeedBoostActive = false;

    [SerializeField]
    private bool _isShieldActive = false;

    [SerializeField]
    private GameObject _shieldVisualizer;

    private int _score;

    [SerializeField]
    private UIManager _uiManager;

    [SerializeField]
    private GameObject _rightEngine, _leftEngine;

    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip _laserSound;


    // Start is called before the first frame update
    void Start()
    {
        // Current position
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager")?.GetComponent<SpawnManager>();

        if (_spawnManager == null)
            Debug.LogError("Spawn_Manager is null");

        _uiManager = GameObject.Find("Canvas")?.GetComponent<UIManager>();
        if (_spawnManager == null)
            Debug.LogError("UIManager is null");

        _audioSource = this.GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on Player is null");
        }
        else
        {
            _audioSource.clip = _laserSound;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
            _audioSource.Play();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Movement
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * _speedMultiplier * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0f), 0);

        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + Vector3.up * 1.05f, Quaternion.identity);
        }
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shieldVisualizer?.SetActive(false);
            return;
        }

        _lives--;
        _uiManager.UpdateLives(_lives);


        switch (_lives)
        {
            case 2:
                bool isRight = Random.value > 0.5;
                if (isRight)
                {
                    _rightEngine.SetActive(true);
                }
                else
                {
                    _leftEngine.SetActive(true);
                }
                break;
            case 1:
                if (_rightEngine.activeSelf)
                {
                    _leftEngine.SetActive(true);
                }
                else
                {
                    _rightEngine.SetActive(true);
                }
                break;
        }
        if (_lives < 1)
        {
            _spawnManager?.OnPlayerDeath(transform.position);
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _isTripleShotActive = false;
    }

    public void SpeedBoost()
    {
        _isSpeedBoostActive = true;
        _speedMultiplier = 2;
        StartCoroutine(SpeedBoostPowerDownRoutine());

    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _isSpeedBoostActive = false;
        _speedMultiplier = 1;
    }

    public void ActivateShield()
    {
        _isShieldActive = true;
        _shieldVisualizer?.SetActive(true);
    }

    public void AddScore(int value)
    {
        _score += value;
        _uiManager?.UpdateScore(_score);
    }
}
