using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class verticalScroll : MonoBehaviour
{
    [Tooltip("Game unit per second")]
    [SerializeField] float scrollRate = 0.1f;
    [SerializeField] bool hasLimit = false;
    [SerializeField] float limitWaterLevel = 10f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float yMove = scrollRate * Time.deltaTime;
        transform.Translate(Vector2.up * yMove);

        if (hasLimit && transform.localPosition.y >= limitWaterLevel)
        {
            scrollRate = 0;
        }
    }
}
