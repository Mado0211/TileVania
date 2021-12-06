using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportBar : MonoBehaviour
{
    GameObject lethalHazard;


    // Start is called before the first frame update
    void Start()
    {

        lethalHazard = transform.parent.Find("Horizontal Bar/Hazard").gameObject;
        lethalHazard.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject, 0.05f);
        lethalHazard.SetActive(true);

        transform.parent.Find("Horizontal Bar").gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    /*
    private void OnJointBreak2D(Joint2D joint)
    {
        Destroy(gameObject, 0.2f);
        lethalHazard.SetActive(true);

        transform.parent.Find("Horizontal Bar").gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }*/
}
