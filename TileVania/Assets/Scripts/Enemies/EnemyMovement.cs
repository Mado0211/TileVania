using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //config
    [SerializeField] float moveSpeed = 1f;

    //cached reference
    Rigidbody2D myRigidbody2D;

    
    
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (IsFacingRight())
        {
            myRigidbody2D.velocity = new Vector2(moveSpeed, 0f);
        }
        else
        {
            myRigidbody2D.velocity = new Vector2(-moveSpeed, 0f);
        }
    }

    bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }
    
    /*
    private void OnTriggerExit2D(Collider2D collision)
    {
        /*if (collision.gameObject.layer != LayerMask.NameToLayer("Player") && collision.gameObject.tag != "Attack Region")
        {
            transform.localScale = new Vector2(-Mathf.Sign(myRigidbody2D.velocity.x), 1f);//轉向
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            transform.localScale = new Vector2(-Mathf.Sign(myRigidbody2D.velocity.x), 1f);//轉向
        }
    }*/

    private void OnDestroy()
    {
        EnemyDashAttack myDashAttack = transform.GetComponentInParent<EnemyDashAttack>();
        if (myDashAttack)
        {
            Destroy(myDashAttack.gameObject);
        }
    }
}
