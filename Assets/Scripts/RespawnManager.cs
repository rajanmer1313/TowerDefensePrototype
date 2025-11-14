using UnityEngine;
using System.Collections;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance;

    [Header("Respawn Settings")]
    public GameObject enemyPrefab;
    public float respawnDelay = 1.5f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SpawnEnemy()
    {
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("RespawnManager: enemyPrefab is NULL");
            yield break;
        }

        yield return new WaitForSeconds(respawnDelay);

        // NEW ENEMY
        GameObject newEnemy = Instantiate(enemyPrefab);
        newEnemy.name = enemyPrefab.name + "_Respawned";

        // FIND PATH PARENT
        Transform path = GameObject.Find("Waypoints")?.transform;

        if (path == null)
        {
            Debug.LogError("Waypoints object NOT found in scene!");
            yield break;
        }

        // SET POSITION
        Transform startPoint = path.GetChild(0);
        newEnemy.transform.position = startPoint.position;

        // GIVE PATH TO ENEMY
        GoblinPathFollower follower = newEnemy.GetComponent<GoblinPathFollower>();

        if (follower != null)
        {
            follower.pathParent = path;
            follower.RespawnAtStart();   // <-- MOVE START KARAO!
        }
        else
        {
            Debug.LogError("No GoblinPathFollower found on respawned enemy!");
        }

        Debug.Log("Respawned enemy started moving!");
    }
}
