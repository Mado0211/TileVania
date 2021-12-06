using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [Header("Player Life")]
    [SerializeField] int playerLives = 3;
    [SerializeField] TMP_Text livesText;
    [SerializeField] float rebornDelay = 1.5f;
    [SerializeField] float reStartGameDelay = 3.0f;

    int score = 0;
    [Header("Score")]
    [SerializeField] Text scoreText;

    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
    }


    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            StartCoroutine(TakeLife());
        }
        else
        {
            StartCoroutine(ResetGameSession());

            AudioClip deadSFX = Resources.Load<AudioClip>("DeathSFX");
            AudioSource.PlayClipAtPoint(deadSFX, Camera.main.transform.position);
        }
    }

    private IEnumerator ResetGameSession()
    {
        playerLives--;
        livesText.text = playerLives.ToString();

        yield return new WaitForSeconds(reStartGameDelay);
        Destroy(gameObject);
        SceneManager.LoadScene("Main menu");
    }


    private IEnumerator TakeLife()
    {
        playerLives--;
        livesText.text = playerLives.ToString();

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        yield return new WaitForSeconds(rebornDelay);
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = score.ToString();
        if (score < 9999999)
        {
            scoreText.text = score.ToString();
        }
        else
        {
            scoreText.text = "9999999";
            Debug.Log("Score has been over the limit.");
        }
    }

    

    public int GetScore()
    {
        return score;
    }

    public void RestoreLives(int amount)
    {
        playerLives += amount;
        livesText.text = playerLives.ToString();
    }
}
