using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    float upSpeed = 20f;
    Vector3 goalPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 1.0f);
        goalPosition = transform.position + Vector3.up * 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != goalPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, goalPosition, upSpeed * Time.deltaTime);
        }
        else
        {

        }
    }
}
