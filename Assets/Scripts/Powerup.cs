using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;

    [SerializeField]
    // 0 - Triple Shot
    // 1 - Speed
    // 2 - Speed
    private int _powerupId;

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


        if (transform.position.y <= -6.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            switch (_powerupId)
            {
                case 0:
                    other.GetComponent<Player>()?.TripleShotActive();
                    break;
                case 1:
                    other.GetComponent<Player>()?.SpeedBoost();
                    break;
                case 2:
                    break;
            }
            Destroy(this.gameObject);
        }
    }
}
