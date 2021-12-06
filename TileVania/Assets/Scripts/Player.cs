using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    //config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(10f, 25f);

    //state
    bool isAlive = true;
    bool isShooting = false;
    bool isOnLadderState = false;

    //cached component reference or so
    Animator myAnimator;
    Rigidbody2D myRigidbody2D;
    CapsuleCollider2D myBodyCollider2D;
    BoxCollider2D myFeetCollider2D;
    float gravityScaleAtStart;

    //cached Equipment, projectile
    PlayerProjectile arrowPrefab;
    PlayerEquipment myEquipment;
    GameObject absorbPowerVFX;
    float startAbsorbTime;
    bool isPowerShoot = false;
    bool isFireButtonDown = false;

    bool unlimitedArrows = false;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myBodyCollider2D = GetComponent<CapsuleCollider2D>();
        myFeetCollider2D = GetComponent<BoxCollider2D>();
        //playerBodyTransform = transform.Find("Body");

        gravityScaleAtStart = myRigidbody2D.gravityScale;

        //cached Equipment, projectile
        arrowPrefab = Resources.Load<PlayerProjectile>("Flying Arrow");
        myEquipment = FindObjectOfType<PlayerEquipment>();
        absorbPowerVFX = transform.Find("Absorb Power VFX").gameObject;
        absorbPowerVFX.SetActive(false);

        if (SceneManager.GetActiveScene().name == "Main menu")
        {
            unlimitedArrows = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!isAlive) { return; }
        Pause();

        Jump();
        

        isShooting = myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Player Shoot");
        if (!isShooting)
        {
            if (!isOnLadderState)
            {
                Shoot();
            }
            
            Run();
            FlipSprite();
            ReadyToClimb();
        }

        
        if (isOnLadderState)
        {
            ClimbLadder();
        }

        Die();

    }

    private void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }

    /// <summary>
    /// player run
    /// </summary>
    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidbody2D.velocity.y);
        myRigidbody2D.velocity = playerVelocity;

        // change to running state
        bool isPlayerHasHorizontalSpeed = Mathf.Abs(myRigidbody2D.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", isPlayerHasHorizontalSpeed);
    }

    private void Jump()
    {
        if (!myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground", "Shootable", "Standing Arrow")))
        {//沒在地上 (在第1格樓梯也是)
            return;
        }

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidbody2D.velocity += jumpVelocityToAdd;
        }
    }

    /// <summary>
    /// change player's face  when move
    /// </summary>
    private void FlipSprite()
    {
        bool isPlayerHasHorizontalSpeed = Mathf.Abs(myRigidbody2D.velocity.x) > Mathf.Epsilon;
        if (isPlayerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody2D.velocity.x), 1);            
        }
    }

    private void ReadyToClimb()
    {
        if (myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {//碰到階梯
            bool getOnLadder = CrossPlatformInputManager.GetButton("Vertical");
            if (getOnLadder)
            {
                myAnimator.SetBool("isRunning", false);
                myAnimator.SetBool("IsOnLadder", true);
                isOnLadderState = true;
                myRigidbody2D.gravityScale = 0f;
            }
            //print("碰到階梯");
        }
        else
        {//沒碰到階梯
            //print("not 碰到階梯");
            if (isOnLadderState)
            {
                LeaveLadder();
            }
        }
    }

    private void LeaveLadder()
    {
        isOnLadderState = false;
        myAnimator.SetBool("isClimbing", false);
        myAnimator.SetBool("IsOnLadder", false);
        myRigidbody2D.gravityScale = gravityScaleAtStart;
        //print("Leave Ladder");
    }

    private void ClimbLadder()
    {
        /*if (!myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {//沒碰到階梯
            myAnimator.SetBool("isClimbing", false);
            
            //myRigidbody2D.gravityScale = gravityScaleAtStart;
            return;
        }*/

        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 climbingVelocity = new Vector2(myRigidbody2D.velocity.x, controlThrow * climbSpeed);
        myRigidbody2D.velocity = climbingVelocity;
        //myRigidbody2D.gravityScale = 0f;

        // change to climb state
        bool isPlayerHasVerticalSpeed = Mathf.Abs(myRigidbody2D.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", isPlayerHasVerticalSpeed);        
    }

    private void Die()
    {
        if (myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            myAnimator.SetTrigger("Die");
            isAlive = false;
            myRigidbody2D.velocity = deathKick;

            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    private void Shoot()
    {
        if (!unlimitedArrows)
        {
            if (!myEquipment) return;

            bool hasArrow = myEquipment.HaveArrows();
            if (!hasArrow) return;
        }

        if (CrossPlatformInputManager.GetButtonDown("Fire1") )
        {
            isFireButtonDown = true;

            //聚集力量
            startAbsorbTime = Time.timeSinceLevelLoad;
            absorbPowerVFX.SetActive(true);
        }

        
        if (CrossPlatformInputManager.GetButton("Fire1"))
        {
            if (absorbPowerVFX.activeSelf && !isPowerShoot)
            {//改變型態
                float absorbTime = Time.timeSinceLevelLoad - startAbsorbTime;
                if (absorbTime > 1.5f)
                {
                    isPowerShoot = true;
                    ChangeParticleType(ParticleSystemShapeType.SingleSidedEdge, .5f);
                    //print("Reach power shoot, Time is " + absorbTime + "s");
                }
            }
        }

        if (CrossPlatformInputManager.GetButtonUp("Fire1") && isFireButtonDown)
        {
            isFireButtonDown = false;

            myAnimator.SetTrigger("shootArrow");
            if (myEquipment)
            {
                myEquipment.UseArrow();
            }

            //釋放力量
            if (isPowerShoot)
            {
                ChangeParticleType(ParticleSystemShapeType.Sphere, 2f);
            }
            absorbPowerVFX.SetActive(false);
        }
    }

    /// <summary>
    /// Change Particle shape Type
    /// </summary>
    /// <param name="shapeType">shape Type</param>
    /// <param name="radius"> radius</param>
    private void ChangeParticleType(ParticleSystemShapeType shapeType, float radius)
    {
        //GetComponent<ParticleSystem>().shape.shapeType = ParticleSystemShapeType.Mesh;
        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
        var sh = ps.shape;
        //sh.enabled = true;
        sh.shapeType = shapeType;
        sh.radius = radius;
    }

    public void ShootArraw()
    {
        Vector2 arrowReadyPos = transform.Find("ArrowPos").position;
        PlayerProjectile arrow = Instantiate(arrowPrefab, arrowReadyPos, Quaternion.identity);
        arrow.transform.localScale = transform.localScale;

        if (isPowerShoot)
        {
            isPowerShoot = false;
            arrow.SetPowerShoot();
        }
    }


}
