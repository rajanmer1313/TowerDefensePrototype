using UnityEngine;
using System.Collections;

public class GoblinPathFollower : MonoBehaviour
{
    [Header("Path Settings")]
    public Transform pathParent; // PathGizmo ke child waypoints ka parent
    public float moveSpeed = 2f;
    public float reachThreshold = 0.2f; // Point ke paas pahunchne ka distance

    [Header("Animator Settings")]
    public Animator animator; // Assign the Animator component
    public string walkParam = "isWalking"; // Animator bool parameter for walking

    [Header("Spawn Offset")]
    public float enemyHeightOffset = -0.5f; // Adjust based on sprite pivot

    private Transform[] waypoints;
    private int currentWaypoint = 0;

    void Start()
    {
        // Waypoints array setup
        if (pathParent != null && pathParent.childCount > 0)
        {
            waypoints = new Transform[pathParent.childCount];
            for (int i = 0; i < pathParent.childCount; i++)
            {
                waypoints[i] = pathParent.GetChild(i);
                Debug.Log("Waypoint " + i + ": " + waypoints[i].position);
            }

            // Start moving
            if (animator != null)
                animator.SetBool(walkParam, true);

            // Spawn at first waypoint + offset
            Vector3 spawnOffset = new Vector3(0f, enemyHeightOffset, 0f);
            transform.position = waypoints[0].position + spawnOffset;

            currentWaypoint = 1;
        }
        else
        {
            Debug.LogWarning("No waypoints assigned for GoblinPathFollower!");
        }
    }

    void Update()
    {
        if (waypoints == null || currentWaypoint >= waypoints.Length) return;

        Vector3 targetPos = waypoints[currentWaypoint].position;

        // Direction on XY plane only
        Vector3 direction = targetPos - transform.position;
        direction.z = 0f;
        direction.Normalize();

        // Move enemy
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Fix Z position
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

        // Flip sprite based on X movement (head up, legs down)
        if (direction.x > 0f)
            transform.localScale = new Vector3(1f, 1f, 1f); // facing right
        else if (direction.x < 0f)
            transform.localScale = new Vector3(-1f, 1f, 1f); // facing left

        // Distance on XY plane only
        float distance = Vector2.Distance(
            new Vector2(transform.position.x, transform.position.y),
            new Vector2(targetPos.x, targetPos.y)
        );

        

        // Check if reached waypoint
        if (distance <= reachThreshold)
        {
           
            currentWaypoint++;

            // Check if reached end of path
            if (currentWaypoint >= waypoints.Length)
            {
                if (animator != null)
                    animator.SetBool(walkParam, false);

                Destroy(gameObject); // Destroy goblin at the end
                
            }
        }
    }
}
