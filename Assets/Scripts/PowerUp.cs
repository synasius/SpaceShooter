using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private enum PowerUpType
    {
        TripleShot,
        Speed,
        Shield
    }

    [SerializeField]
    private float _speed = 3f;
    [SerializeField]
    private PowerUpType _powerUpType;

    [SerializeField]
    private AudioClip _powerUpSound;

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -7f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();

        if (player)
        {
            AudioSource.PlayClipAtPoint(_powerUpSound, transform.position);
            switch (_powerUpType)
            {
                case PowerUpType.TripleShot:
                    player.ActivateTripleShot();
                    break;
                case PowerUpType.Speed:
                    player.ActivateSpeed();
                    break;
                case PowerUpType.Shield:
                    player.ActivateShield();
                    break;
            }
            Destroy(gameObject);
        }
    }
}
