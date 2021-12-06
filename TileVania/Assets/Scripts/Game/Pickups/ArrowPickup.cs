using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPickup : MonoBehaviour
{
    AudioClip pickupSFX;
    [SerializeField] float soundVolume = 0.8f;
    [SerializeField] int arrowNumber = 1;
    [SerializeField] bool backupArrow = false;
    bool isPicked = false;
    

    // Start is called before the first frame update
    void Start()
    {
        pickupSFX = Resources.Load<AudioClip>("SFX-Heart-Pickup");
        if (FindObjectOfType<PlayerEquipment>().ArrowNumber > 0 && backupArrow)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPicked)
        {
            isPicked = true;
            if (collision.GetComponent<Player>().IsAlive())
            {
                FindObjectOfType<PlayerEquipment>().ReceiveArrow(arrowNumber);
            }
            else
            {
                //FindObjectOfType<GameSession>().RestoreLives(heartRestorePoint * 3);
                Debug.Log("get it after death");
            }

            AudioSource.PlayClipAtPoint(pickupSFX, Camera.main.transform.position, soundVolume);
            Destroy(gameObject);
        }
    }
}
