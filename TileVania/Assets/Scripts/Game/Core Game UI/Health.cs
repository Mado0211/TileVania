using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float health = 100;
    [SerializeField] GameObject deathVFX;
    [SerializeField] GameObject attackedVFX;

    private float Max_Health;
    Transform healthBar;

    private void Start()
    {
        InitializeHealthBar();
        UpdateHealthBar();
    }

    private void InitializeHealthBar()
    {
        Max_Health = health;
        healthBar = transform.Find("HealthBar/Fill Area");
        if (!healthBar)
        {
            Debug.LogWarning(gameObject.name + ", Can't find the health bar fill area!");
        }
    }

    /// <summary>
    /// cause a damage to it
    /// </summary>
    /// <param name="damage">how much damage does it caused</param>
    public void DealDamage(float damage)
    {
        health -= damage;
        UpdateHealthBar();
        TriggerGetAttackedVFX();

        if (health <= 0)
        {
            TriggerDeathVFX();
            //Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
            
        }
    }


    private void TriggerDeathVFX()
    {
        if (!deathVFX) { return; }
        GameObject deathVFXObecjt = Instantiate(deathVFX, transform.position, Quaternion.identity);
        Destroy(deathVFXObecjt, 0.5f);
    }

    private void TriggerGetAttackedVFX()
    {
        if (!attackedVFX) { return; }
        GameObject attackedVFXObecjt = Instantiate(attackedVFX, transform.position, Quaternion.identity);
        Destroy(attackedVFXObecjt, 0.5f);
    }

    private void UpdateHealthBar()
    {
        float healthPercentage = health / Max_Health;
        healthBar.localScale = new Vector3(healthPercentage, 1, 1);
    }

    public float GetHealth()
    {
        return health;
    }
}
