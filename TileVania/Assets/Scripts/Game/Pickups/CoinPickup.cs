using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinPickup : MonoBehaviour
{
    AudioClip pickupSFX;
    [SerializeField] float soundVolume = 0.8f;
    [SerializeField] int pointsForCoinPickup = 100;
    bool isPicked = false;

    [Header("Reward")]
    [SerializeField] int diePickFactor = 100;
    [SerializeField] string diePickText = "Death is Money";

    // Start is called before the first frame update
    void Start()
    {
        pickupSFX = Resources.Load<AudioClip>("PickupSoundEffect");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPicked)
        {
            isPicked = true;
            if (collision.GetComponent<Player>().IsAlive())
            {
                FindObjectOfType<GameSession>().AddToScore(pointsForCoinPickup);
            }
            else
            {
                if (!transform.Find("Text"))
                {
                    FindObjectOfType<GameSession>().AddToScore(pointsForCoinPickup);
                    print("This one does has a Text, but pick it after death.");
                }
                else
                {
                    FindObjectOfType<GameSession>().AddToScore(pointsForCoinPickup * diePickFactor);
                    //ShowAchievementText();
                    ScoreFctorText();
                }
            }
            AudioSource.PlayClipAtPoint(pickupSFX, Camera.main.transform.position, soundVolume);
            Destroy(gameObject);
        }
    }

    private void ScoreFctorText()
    {
        GameObject myText = transform.Find("Text").gameObject;

        GameObject factorAmountText = Instantiate(myText,
            myText.transform.position, Quaternion.identity);
        factorAmountText.GetComponent<Text>().text = "X" + diePickFactor.ToString();
        factorAmountText.transform.SetParent(transform.parent);

        //x10Text.transform.Translate(Vector2.up * TextupSpeed * Time.deltaTime);
        factorAmountText.AddComponent<FloatingText>();
    }

    private void ShowAchievementText()
    {
        GameObject myText = transform.Find("Text").gameObject;

        GameObject achievementGameObecjt = Instantiate(myText,
            transform.position, Quaternion.identity);

        achievementGameObecjt.transform.SetParent(transform.parent);

        Text achievementText = achievementGameObecjt.GetComponent<Text>();
        achievementText.font = Resources.Load<Font>("Hallowen Scare St");
        achievementText.fontStyle = FontStyle.Italic;
        achievementText.text = diePickText;
        achievementText.fontSize = 7;
    }
}
