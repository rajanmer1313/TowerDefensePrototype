using UnityEngine;

public class ArrowShooter : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject bulletPrefab;    // Bullet prefab
    public Transform firePoint;        // FirePoint
    public float fireRate = 1f;        // Shots per second
    public float bulletSpeed = 10f;    // Speed of bullet

    [Header("Detection Settings")]
    public float range = 6f;           // Attack range
    public string enemyTag = "Enemy";  // Enemy tag

    private float fireCooldown = 0f;
    private Transform target;

    void Update()
    {
        fireCooldown -= Time.deltaTime;
        UpdateTarget();

        if (target != null && fireCooldown <= 0f)
        {
            Shoot();
            fireCooldown = 1f / fireRate;
        }
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= range)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        target = nearestEnemy != null ? nearestEnemy.transform : null;
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null || target == null)
            return;

        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Rotate the arrow towards the target
        Vector3 dir = (target.position - firePoint.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        bulletGO.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

        Bullet3D bullet = bulletGO.GetComponent<Bullet3D>();
        if (bullet != null)
        {
            bullet.SetTarget(target);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
