using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SuccessScreen : MonoBehaviour
{
    Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        GameSession gameSession = FindObjectOfType<GameSession>();
        scoreText = transform.Find("Score Text").GetComponent<Text>();
        scoreText.text = "Score: " + gameSession.GetScore().ToString();
        Destroy(gameSession.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main menu");
    }
}
