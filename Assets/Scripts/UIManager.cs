using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Globalization;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _p1ScoreText;
    [SerializeField] private TextMeshProUGUI _p2ScoreText;
    [SerializeField] private Sprite[] _lives;
    [SerializeField] private Image _p1LivesImage;
    [SerializeField] private Image _p2LivesImage;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartText;
    [SerializeField] GameObject _audioManager;
    private int _scene;

    void Start()
    {
        _scene = SceneManager.GetActiveScene().buildIndex;
        if(!MainMenu._musicOn)  _audioManager.SetActive(false);
        SetScore("Player_1",0);
        SetLives("Player_1",3);
        if(_scene==2)
        {
            SetScore("Player_2",0);
            SetLives("Player_2",3);
        }    
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
    }

    public void SetScore(string player,int score)
    {
        if(player=="Player_1")
            _p1ScoreText.text="Score: " + score.ToString();
        else
            _p2ScoreText.text="Score: " + score.ToString();
    }

    public void SetLives(string player,int lives)
    {
        if(player=="Player_1")
            _p1LivesImage.sprite=_lives[lives];
        else
            _p2LivesImage.sprite=_lives[lives];
        if(lives==0)
            StartCoroutine(GameOverSequence());
    }

    IEnumerator GameOverSequence()
    {
        bool flip=false;

        if(_scene==2)   
        {
            string winner = (int.Parse(_p1ScoreText.text.Split(' ')[1], NumberStyles.AllowLeadingSign | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite))
                        >
                        (int.Parse(_p2ScoreText.text.Split(' ')[1],NumberStyles.AllowLeadingSign | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite))?
                        "Player 1 Wins!":
                        (int.Parse(_p2ScoreText.text.Split(' ')[1],NumberStyles.AllowLeadingSign | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite))
                        >
                        (int.Parse(_p1ScoreText.text.Split(' ')[1], NumberStyles.AllowLeadingSign | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite))?
                        "Player 2 Wins!":
                        "It's a Draw!";
            _gameOverText.text = "Game Over!\n" + winner;
        }
        _restartText.gameObject.SetActive(true);
        GameManager gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if(gameManager==null) Debug.Log("Game Manager not found");
        gameManager.GameOver();
        while(true)
        {
            flip=!flip;
            _gameOverText.gameObject.SetActive(flip);
            yield return new WaitForSeconds(0.5f);
        }   
    }
}
