﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private Player _player;

    private Animator _anim;

    private BoxCollider2D _collider;

    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player")?.GetComponent<Player>();
        if (_player == null)
            Debug.LogError("Player is null");

        _anim = this.GetComponent<Animator>();
        if (_anim == null)
            Debug.LogError("Animator is null");

        _collider = this.GetComponent<BoxCollider2D>();
        if (_collider == null)
            Debug.LogError("Collider is null");

        _audioSource = this.GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on Enemy is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        // Movement
        transform.Translate(Vector3.down * _speed * Time.deltaTime);


        if (transform.position.y <= -6.5f)
        {
            float randomX = Random.Range(-10f, 10f);
            transform.position = new Vector3(randomX, 8.0f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player" && other.tag != "Laser")
            return;

        _collider.enabled = false;
        _anim.SetTrigger("OnEnemyDeath");
        if (other.tag == "Player")
        {
            other.GetComponent<Player>()?.Damage();
        }
        else if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _player?.AddScore(10);
        }
        _speed = 0;
        Destroy(this.gameObject, 2.8f);
        _audioSource.Play();
    }
}
