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

    [SerializeField]
    private UIPlayerManager _uiManager;

    [SerializeField]
    private bool _isPlayerTwo = false;

    private string _horizontalAxis = "Horizontal";
    private string _verticalAxis = "Vertical";
    private KeyCode _fireButton = KeyCode.Space;

    private GameManager _gameManager;

    private AudioSource _laserSound;

    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        if (_isPlayerTwo)
        {
            _horizontalAxis = "Horizontal2";
            _verticalAxis = "Vertical2";
            _fireButton = KeyCode.LeftShift;
        }
        _animator = GetComponent<Animator>();

        if (Random.value > 0.5)
        {
            _engines = new GameObject[] { _rightEngine, _leftEngine };
        }
        else
        {
            _engines = new GameObject[] { _leftEngine, _rightEngine };
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

        if (Input.GetKeyDown(_fireButton) && Time.time > _canFire)
        {
            Firing();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis(_horizontalAxis);
        float verticalInput = Input.GetAxis(_verticalAxis);

        _animator.SetBool("Left", horizontalInput < 0);
        _animator.SetBool("Right", horizontalInput > 0);
        _animator.SetFloat("Motion", Mathf.Abs(horizontalInput));
        _animator.SetFloat("Movement", horizontalInput);

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

        GameObject shot = Instantiate(
            weaponPrefab,
            new Vector3(transform.position.x, transform.position.y + _laserOffset),
            Quaternion.identity
        );
        // Attach player information to the lasers so that they know which player
        // has shoot and add score to correct one if the shot hits an enemy.
        foreach (Laser laser in shot.GetComponentsInChildren<Laser>())
        {
            laser.player = this;
        }

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
            _gameManager.PlayerDead();
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
