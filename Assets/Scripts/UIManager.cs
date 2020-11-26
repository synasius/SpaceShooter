using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField]
    private Text _bestScoreLabel;

    [SerializeField]
    private GameObject _pauseMenu;

    [SerializeField]
    private Text _gameOverMessage;

    [SerializeField]
    private Text _restartText;

    void Start()
    {
        _pauseMenu.SetActive(false);
        _gameOverMessage.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
    }

    public void ShowGameOver()
    {
        StartCoroutine("GameOverFlicking");
        _restartText.gameObject.SetActive(true);
    }

    IEnumerator GameOverFlicking()
    {
        while (true)
        {
            _gameOverMessage.gameObject.SetActive(true);
            yield return new WaitForSeconds(Random.value);
            _gameOverMessage.gameObject.SetActive(false);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        }
    }

    public void ShowPauseMenu()
    {
        _pauseMenu.SetActive(true);
    }

    public void HidePauseMenu()
    {
        _pauseMenu.SetActive(false);
    }

    public void SetBestScore(int bestScore)
    {
        if (_bestScoreLabel != null)
        {
            _bestScoreLabel.text = "Best: " + bestScore;
        }
    }
}
