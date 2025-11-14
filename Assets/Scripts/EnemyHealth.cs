using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Respawn Settings")]
    public float respawnDelay = 1.5f;
    // assign your prefab asset (not scene instance) in inspector for this prefab
    public GameObject enemyPrefab;

    void Start()
    {
            currentHealth = maxHealth;

            // Auto-assign prefab asset if null (using Resources or name)
            if (enemyPrefab == null)
            {
                // Try find prefab by name in Resources (if your prefabs are under a Resources folder)
                GameObject found = Resources.Load<GameObject>(name.Replace("(Clone)", "").Trim());
                if (found != null)
                {
                    enemyPrefab = found;
                    Debug.Log($"Auto-assigned prefab for {name}");
                }
                else
                {
                    Debug.LogError($"EnemyHealth: enemyPrefab not assigned and could not find prefab by name '{name}'. Please assign manually in prefab.");
                }
            }

    }

    public void ResetHealthToMax()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        if (RespawnManager.Instance != null)
            RespawnManager.Instance.SpawnEnemy();
        else
            Debug.LogError("EnemyHealth.Die: RespawnManager.Instance is null; can't respawn.");

        Destroy(gameObject);
    }

}
