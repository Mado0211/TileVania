using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BreakableArrow : MonoBehaviour
{
    [SerializeField] float endureLimit = 30f;
    float endureAmount = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        endureAmount += Mathf.Abs(collision.contacts[0].normalImpulse);

        if (endureAmount > endureLimit)
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            GetComponent<Rigidbody2D>().gravityScale = 1;

            Destroy(gameObject, 0.8f);
        }
    }


    public void InitializeEndureLimit(float arrowSpeed)
    {
        endureLimit += arrowSpeed;
    }
}
