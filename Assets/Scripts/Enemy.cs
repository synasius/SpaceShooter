using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;

    [SerializeField]
    private GameObject _enemyLaserPrefab;

    private Animator _animator;
    private AudioSource _explosionSound;

    private IEnumerator _firingCoroutine;

    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        _explosionSound = GetComponent<AudioSource>();

        _firingCoroutine = FiringRoutine();
        StartCoroutine(_firingCoroutine);
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -7f)
        {
            transform.position = new Vector3(Random.Range(-10f, 10f), 11f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            player.Damage();
            _explosionSound.Play();
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0.5f;

            DestroyEnemy();
        }

        if (other.CompareTag("Laser"))
        {
            Laser laser = other.GetComponent<Laser>();
            if (laser.player != null)
            {
                laser.player.AddScore(10);
            }
            _explosionSound.Play();
            Destroy(other.gameObject);
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0.5f;

            DestroyEnemy();
        }
    }

    private void DestroyEnemy()
    {
        StopCoroutine(_firingCoroutine);
        Destroy(GetComponent<CompositeCollider2D>());
        foreach (Collider2D collider in GetComponents<Collider2D>())
        {
            Destroy(collider);
        }
        Destroy(gameObject, 2.8f);
    }

    IEnumerator FiringRoutine()
    {
        while (true)
        {
            // Fire every 3 to 7 seconds
            yield return new WaitForSeconds(Random.Range(3f, 7f));
            Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
        }
    }
}
