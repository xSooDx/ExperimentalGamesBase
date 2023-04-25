using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public bool isShielded = false;

    public UnityEvent onHealthChange;

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
        Debug.Log($"{amount} - {currentHealth}, {gameObject.name}");
        onHealthChange.Invoke();
    }

    void Die()
    {
        // Spawn Death effect
        if (gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
}