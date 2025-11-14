using UnityEngine;

public interface ITower
{
    int GetLevel();
    int GetMaxLevel();
    void Upgrade();
    GameObject GetNextLevelBullet();
    string GetTowerName();
}