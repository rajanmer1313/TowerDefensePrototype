using UnityEngine;

[System.Serializable]
public class TowerLevelData
{
    public GameObject bulletPrefab; // Level specific bullet prefab
    public float range;
    public float fireRate;
    public int damage;
}