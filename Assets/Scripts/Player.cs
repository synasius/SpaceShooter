using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleLaserPrefab;

    [SerializeField]
    private float _speed = 3.0f;
    private float _speedBoost = 1.0f;

    [SerializeField]
    private float _laserOffset = 0.8f;

    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;

    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private bool _isTripleShotActive = false;

    [SerializeField]
    private bool _isShieldActive = false;

    [SerializeField]
    private GameObject _shield;

    [SerializeField]
    private GameObject _rightEngine;

    [SerializeField]
    private GameObject _leftEngine;

    // This array defines the order in which engines are destroyed
    private GameObject[] _engines;

    [SerializeField]
    private int _score = 0;

    private UIManager _uiManager;
    private GameManager _gameManager;

    private AudioSource _laserSound;

    // Start is called before the first frame update
    void Start()
    {
        if (Random.value > 0.5)
        {
            _engines = new GameObject[] { _rightEngine, _leftEngine };
        }
        else
        {
            _engines = new GameObject[] { _leftEngine, _rightEngine };
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogAssertion("The UIManager is Null");
        }

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogAssertion("The GameManager is Null");
        }

        _laserSound = GetComponent<AudioSource>();
        if (_laserSound == null)
        {
            Debug.LogAssertion("The Laser Sound is Null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            Firing();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * _speedBoost * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.7f, 5.8f), 0);
        if (transform.position.x >= 12.0)
        {
            transform.position = new Vector3(-12.0f, transform.position.y, 0);
        }
        else if (transform.position.x <= -12.0)
        {
            transform.position = new Vector3(12.0f, transform.position.y, 0);
        }
    }

    void Firing()
    {
        _canFire = Time.time + _fireRate;
        var weaponPrefab = _laserPrefab;
        if (_isTripleShotActive)
        {
            weaponPrefab = _tripleLaserPrefab;
        }

        Instantiate(
            weaponPrefab,
            new Vector3(transform.position.x, transform.position.y + _laserOffset),
            Quaternion.identity
        );

        _laserSound.Play();
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shield.SetActive(false);
            return;
        }

        _lives -= 1;
        _uiManager.SetLives(_lives);

        if (_lives == 2)
        {
            _engines[0].SetActive(true);
        }

        if (_lives == 1)
        {
            _engines[1].SetActive(true);
        }

        if (_lives < 1)
        {
            _gameManager.GameOver();
            Destroy(gameObject);
        }
    }

    public void ActivateTripleShot()
    {
        StopCoroutine("TripleShotDeactivate");
        _isTripleShotActive = true;
        StartCoroutine("TripleShotDeactivate");
    }

    IEnumerator TripleShotDeactivate()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
    }

    public void ActivateSpeed()
    {
        StopCoroutine("SpeedDeactivate");
        _speedBoost = 2.0f;
        StartCoroutine("SpeedDeactivate");
    }

    IEnumerator SpeedDeactivate()
    {
        yield return new WaitForSeconds(5f);
        _speedBoost = 1.0f;
    }

    public void ActivateShield()
    {
        _isShieldActive = true;
        _shield.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.SetScore(_score);
    }
}
