using UnityEngine;
using UnityEngine.UI;

public class UIPlayerManager : MonoBehaviour
{

    [SerializeField]
    private Text _scoreLabel;

    [SerializeField]
    private Image _livesImage;

    [SerializeField]
    private Sprite[] _liveSprites;

    void Start()
    {
        _scoreLabel.text = "Score: " + 0;
    }

    public void SetScore(int score)
    {
        _scoreLabel.text = "Score: " + score;
    }

    public void SetLives(int currentLives)
    {
        _livesImage.sprite = _liveSprites[currentLives];
    }
}
