using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [Header("Normal")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] int damagePoint = 50;

    [Header("Power up")]
    [SerializeField] float powerUpMoveSpeed = 80f;
    [SerializeField] int powerUpDamagePoint = 300;

    bool hasHit = false;
    int direction;

    // Start is called before the first frame update
    void Start()
    {
        direction = (int)Mathf.Sign(transform.localScale.x);
        GetComponent<Rigidbody2D>().AddForce(Vector2.right * direction * moveSpeed, ForceMode2D.Impulse);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HittingResult(collision.gameObject);
        print("Collision Hit: " + LayerMask.LayerToName(collision.gameObject.layer));
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HittingResult(collision.gameObject);
        print("Trigger Hit: " + LayerMask.LayerToName(collision.gameObject.layer));
    }


    private void HittingResult(GameObject collidedObject)
    {
        if (!hasHit)
        {
            hasHit = true;
            ShowHittedVFX();
            

            if (collidedObject.layer == LayerMask.NameToLayer("Ground"))
            {
                PintoWall();
            }
            else
            {
                Health health = collidedObject.GetComponent<Health>();
                if (health)
                {
                    health.DealDamage(damagePoint);
                }

                Destroy(gameObject);
            }
        }
    }



    private void ShowHittedVFX()
    {
        GameObject hitVFXPrefab = Resources.Load<GameObject>("HitVFX") as GameObject;

        Vector3 VFXPositoin = transform.Find("VFXPositoin").position;
        GameObject myHitVFX = Instantiate(hitVFXPrefab,
            VFXPositoin, Quaternion.identity);

        Destroy(myHitVFX, 0.5f);
    }

    private void PintoWall()
    {
        gameObject.layer = LayerMask.NameToLayer("Standing Arrow");
        GetComponent<BoxCollider2D>().isTrigger = false;

        //Destroy(GetComponent<Rigidbody2D>());
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        gameObject.AddComponent<BreakableArrow>().InitializeEndureLimit(moveSpeed);
        Destroy(this);
    }

    public void SetPowerShoot()
    {
        moveSpeed = powerUpMoveSpeed;
        damagePoint = powerUpDamagePoint;
    }


}
