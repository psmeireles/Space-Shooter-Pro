using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        
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


        if (transform.position.y <= -3.8f)
        {
            float randomX = Random.Range(-10f, 10f);
            transform.position = new Vector3(randomX, 8.0f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Player>()?.Damage();
        }
        else if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
        }
        Destroy(this.gameObject);
    }
}
