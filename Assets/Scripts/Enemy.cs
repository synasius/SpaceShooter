using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;

    private Player _player;

    private Animator _animator;
    private AudioSource _explosionSound;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = gameObject.GetComponent<Animator>();
        _explosionSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
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
            _explosionSound.Play();
            _player.Damage();
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0.5f;

            DestroyEnemy();
        }

        if (other.CompareTag("Laser"))
        {
            _explosionSound.Play();
            Destroy(other.gameObject);
            _player.AddScore(10);
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0.5f;

            DestroyEnemy();
        }
    }

    private void DestroyEnemy()
    {
        Destroy(GetComponent<CompositeCollider2D>());
        foreach(Collider2D collider in GetComponents<Collider2D>()) 
        {
            Destroy(collider);
        }
        Destroy(gameObject, 2.8f);
    }
}
