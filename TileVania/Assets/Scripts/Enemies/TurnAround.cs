using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnAround : MonoBehaviour
{
    Rigidbody2D myRigidbody2D;
    EnemyMovement enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<EnemyMovement>();
        if (!enemy) { print("I can't move."); }
        else
        {
            myRigidbody2D = enemy.GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            enemy.transform.localScale = new Vector2(-Mathf.Sign(myRigidbody2D.velocity.x), 1f);//轉向
        }
    }
}
