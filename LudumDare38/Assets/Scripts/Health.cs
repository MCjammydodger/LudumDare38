using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    public float maxHealth;
    public float currentHealth;

    [SerializeField]
    private GameObject explosionPrefab;

    public delegate void OnHealthChanged(float newHealth);
    public event OnHealthChanged HealthChangedEvent;

    public delegate void OnDeath();
    public event OnDeath DeathEvent;

    private bool dead;

    private void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        if (HealthChangedEvent != null)
        {
            HealthChangedEvent(currentHealth);
        }
        if(currentHealth <= 0 && !dead)
        {
            Die();
        }
    }

    private void Die()
    {
        dead = true;
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(explosion, 5f);
        }
        //gameObject.SetActive(false);
        if (DeathEvent != null)
        {
            DeathEvent();
        }
        Destroy(gameObject);
    }
}
