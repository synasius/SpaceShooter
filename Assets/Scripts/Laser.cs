using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10.0f;

    [SerializeField]
    private bool _isEnemyLaser = false;

    public Player player = null;

    private Vector3 _direction = Vector3.up;

    void Start()
    {
        // When this instance is an enemy laser it will move down
        if (_isEnemyLaser)
        {
            _direction = Vector3.down;
        }
    }

    void Update()
    {
        transform.Translate(_direction * _speed * Time.deltaTime);

        if (transform.position.y > 10.0f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();

        if (player != null && _isEnemyLaser)
        {
            player.Damage();
            Destroy(gameObject);
        }
    }
}
