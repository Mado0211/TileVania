using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDashAttack : MonoBehaviour
{
    [Header("Dash Target")]
    [SerializeField] bool isDashToPlayer = true;
    [SerializeField] float dashSpeed = 5f;

    [Header("Dash to Player")]    
    [SerializeField] float dashTime = 1f;
    [SerializeField] float dashResetTime = 0.2f;

    [Header("Random Dash")]
    [SerializeField] float maxDashTime = 5f;
    [SerializeField] float minDashTime = 0.5f;
    [SerializeField] float maxDashInterval = 6f;
    [SerializeField] float minDashInterval = 2f;

    Collider2D attackRigion;
    EnemyMovement enemy;
    Animator enemyAnimator;
    
    float originalMoveSpeed;
    bool isDashing = false;

    // Start is called before the first frame update
    void Start()
    {
        attackRigion = transform.Find("Attack Region").GetComponent<Collider2D>();
        if (!attackRigion) { print("I don't have a Attack Region"); }

        enemy = GetComponentInChildren<EnemyMovement>();
        originalMoveSpeed = enemy.MoveSpeed;
        if (originalMoveSpeed == 0) { originalMoveSpeed = 1f; }

        enemyAnimator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashToPlayer)
        {
            UpdateAttackRegion();
        }
        else
        {
            RandomRash();
        }
    }

    private void UpdateAttackRegion()
    {
        if (attackRigion)
        {
            attackRigion.transform.position = enemy.transform.position;
            attackRigion.transform.localScale = enemy.transform.localScale;
            DetectPlayer();
        }
    }

    private void DetectPlayer()
    {
        if (attackRigion.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            //print("player is inside");
            if (!isDashing)
            {
                StartCoroutine(Dash(dashTime, dashResetTime, 0));
            }
        }
    }

    private IEnumerator Dash(float dashTime, float dashResetTime, float dashInterval)
    {
        isDashing = true;
        enemy.MoveSpeed = dashSpeed;
        enemyAnimator.SetBool("isResting", false);

        yield return new WaitForSeconds(dashTime);

        enemyAnimator.SetBool("isResting", true);
        enemy.MoveSpeed = 0;       
        yield return new WaitForSeconds(dashResetTime);

        enemyAnimator.SetBool("isResting", false);
        enemy.MoveSpeed = originalMoveSpeed;

        yield return new WaitForSeconds(dashInterval);
        isDashing = false;
    }

    private void RandomRash()
    {
        if (!isDashing)
        {
            float dashTime, dashResetTime, dashInterval;
            dashTime = Random.Range(minDashTime, maxDashTime);
            dashResetTime = dashTime / 2;
            dashInterval = Random.Range(minDashInterval, maxDashInterval);

            StartCoroutine(Dash(dashTime, dashResetTime, dashInterval));
        }
    }
}
