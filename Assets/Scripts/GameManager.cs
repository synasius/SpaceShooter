using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private bool _isGameOver = false;

    [SerializeField]
    private bool _isCoOpMode = false;

    private int _deadPlayers = 0;

    void Start()
    {
        _isGameOver = false;

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogAssertion("The Spawn Manager is Null");
        }
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogAssertion("The UIManager is Null");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void PlayerDead()
    {
        _deadPlayers += 1;

        if (_isCoOpMode && _deadPlayers < 2)
        {
            return;
        }

        GameOver();
    }

    private void GameOver()
    {
        _isGameOver = true;

        _spawnManager.OnPlayerDeath();
        _uiManager.ShowGameOver();
    }
}
