using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    AudioClip pickupSFX;
    [SerializeField] float soundVolume = 0.8f;
    [SerializeField] int heartRestorePoint = 1;
    bool isPicked = false;


    // Start is called before the first frame update
    void Start()
    {
        pickupSFX = Resources.Load<AudioClip>("SFX-Heart-Pickup");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPicked)
        {
            if (collision.GetComponent<Player>().IsAlive())
            {
                FindObjectOfType<GameSession>().RestoreLives(heartRestorePoint);
                isPicked = true;
            }
            else
            {
                //FindObjectOfType<GameSession>().RestoreLives(heartRestorePoint * 3);
                Debug.Log("restore heart after death");
            }

            AudioSource.PlayClipAtPoint(pickupSFX, Camera.main.transform.position, soundVolume);
            Destroy(gameObject);
        }
    }
}
