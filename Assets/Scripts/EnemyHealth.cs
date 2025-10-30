using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Health Settings")]
    public int maxHealth;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    // Called by bullets when they hit the enemy
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(" damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Optional: play animation, sound, or particles before destroy
        Destroy(gameObject);
    }
}
