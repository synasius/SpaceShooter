using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private bool _isGameOver = false;
    private bool _isGamePaused = false;

    private int _bestScore = 0;

    [SerializeField]
    private bool _isCoOpMode = false;

    private int _deadPlayers = 0;

    private void Awake()
    {
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
        _bestScore = PlayerPrefs.GetInt("best", 0);
    }

    void Start()
    {
        _uiManager.SetBestScore(_bestScore);
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

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (_isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PlayerDead()
    {
        _deadPlayers += 1;

        if (_isCoOpMode && _deadPlayers < 2)
        {
            return;
        }

        PlayerPrefs.SetInt("best", _bestScore);
        GameOver();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void CheckBestScore(int current)
    {
        if (current > _bestScore)
        {
            _bestScore = current;
            _uiManager.SetBestScore(_bestScore);
        }
    }

    private void GameOver()
    {
        _isGameOver = true;

        _spawnManager.OnPlayerDeath();
        _uiManager.ShowGameOver();
    }

    private void PauseGame()
    {
        _isGamePaused = true;
        Time.timeScale = 0;
        _uiManager.ShowPauseMenu();
    }

    private void ResumeGame()
    {
        _isGamePaused = false;
        Time.timeScale = 1;
        _uiManager.HidePauseMenu();
    }
}
