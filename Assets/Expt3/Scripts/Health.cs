using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public bool isShielded = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        // Spawn Death effect
        if(gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(this.gameObject);
        }
        
    }
}