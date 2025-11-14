using UnityEngine;

public class TowerShooter3D : MonoBehaviour, ITower
{
    [Header("Tower Levels Settings")]
    public TowerLevelData[] levels;
    private int currentLevel = 0;

    [Header("Fire Point")]
    public Transform firePoint;

    [Header("Enemy Detection")]
    public string enemyTag = "Enemy";
    private Transform target;
    private float fireCooldown = 0f;

    // ================= ITower IMPLEMENTATION =================

    public int GetLevel() => currentLevel;
    public int GetMaxLevel() => levels.Length;
    public string GetTowerName() => gameObject.name;

    public void Upgrade()
    {
        if (currentLevel < levels.Length - 1)
        {
            currentLevel++;
            Debug.Log($"{name} upgraded to Level {currentLevel + 1}");
        }
        else
        {
            Debug.Log($"{name} already at max level!");
        }
    }

    public GameObject GetNextLevelBullet()
    {
        if (currentLevel < levels.Length - 1)
            return levels[currentLevel + 1].bulletPrefab;

        return null;
    }

    // ================= SHOOTING LOGIC =================

    void Update()
    {
        fireCooldown -= Time.deltaTime;
        UpdateTarget();

        if (target != null && fireCooldown <= 0f)
        {
            Shoot();
            fireCooldown = 1f / levels[currentLevel].fireRate;
        }
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortest = Mathf.Infinity;
        GameObject nearest = null;

        foreach (var enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < shortest && dist <= levels[currentLevel].range)
            {
                shortest = dist;
                nearest = enemy;
            }
        }

        target = nearest ? nearest.transform : null;
    }

    void Shoot()
    {
        var data = levels[currentLevel];
        if (data.bulletPrefab == null || firePoint == null || target == null)
            return;

        GameObject bulletGO = Instantiate(data.bulletPrefab, firePoint.position, Quaternion.identity);

        Bullet3D bullet = bulletGO.GetComponent<Bullet3D>();
        if (bullet)
        {
            bullet.SetTarget(target);
            bullet.damage = data.damage;
        }
    }

    // ================= CLICK  OPEN PANEL =================
    void OnMouseDown()
    {
        if (TowerUpgradePanel.Instance == null)
        {
            Debug.LogError("UPGRADE PANEL INSTANCE IS NULL!");
            return;
        }

        TowerUpgradePanel.Instance.OpenPanel(this);
    }


    void OnDrawGizmosSelected()
    {
        if (levels != null && levels.Length > 0)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, levels[currentLevel].range);
        }
    }
}
