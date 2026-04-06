using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text paddle1ScoreText;
    [SerializeField] private TMP_Text paddle2ScoreText;
    [SerializeField] private Transform paddle1Transform;
    [SerializeField] private Transform paddle2Transform;
    [SerializeField] private Transform ballTransform;
    [SerializeField] private int scoreToWin = 10;
    [SerializeField] private string playerWinScene = "YouWIN";
    [SerializeField] private string aiWinScene = "Game Over";
    [SerializeField] private float endSceneDelay = 1f;

    private int paddle1Score;
    private int paddle2Score;
    private bool gameOver;
    private Coroutine endGameCoroutine;

    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<GameManager>();
            }
            return instance;

        }
    }

    public bool GameOver => gameOver;

    private void Start()
    {
        ResetMatch();
    }

    public void Paddle1Scored()
    {
        if (gameOver)
        {
            return;
        }

        paddle1Score++;
        paddle1ScoreText.text = paddle1Score.ToString();
        CheckForWinner();

    }

    public void Paddle2Scored()
    {
        if (gameOver)
        {
            return;
        }

        paddle2Score++;
        paddle2ScoreText.text = paddle2Score.ToString();
        CheckForWinner();

    }

    public void Restart()
    {
        paddle1Transform.position = new Vector2(paddle1Transform.position.x, 0);
        paddle2Transform.position = new Vector2(paddle2Transform.position.x, 0);
        ballTransform.position = Vector2.zero;
    }

    public void ResetMatch()
    {
        paddle1Score = 0;
        paddle2Score = 0;
        gameOver = false;

        paddle1ScoreText.text = paddle1Score.ToString();
        paddle2ScoreText.text = paddle2Score.ToString();

        Restart();
    }

    private void CheckForWinner()
    {
        if (paddle1Score >= scoreToWin)
        {
            gameOver = true;
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayWin();
            }
            Debug.Log("Paddle 1 wins!");
            LoadEndScene(playerWinScene);
        }
        else if (paddle2Score >= scoreToWin)
        {
            gameOver = true;
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayLose();
            }
            Debug.Log("Paddle 2 wins!");
            LoadEndScene(aiWinScene);
        }
    }

    private void LoadEndScene(string sceneName)
    {
        if (endGameCoroutine != null)
        {
            StopCoroutine(endGameCoroutine);
        }

        endGameCoroutine = StartCoroutine(LoadEndSceneAfterDelay(sceneName));
    }

    private IEnumerator LoadEndSceneAfterDelay(string sceneName)
    {
        yield return new WaitForSeconds(endSceneDelay);
        SceneManager.LoadScene(sceneName);
    }
}
