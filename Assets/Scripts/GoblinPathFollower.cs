using UnityEngine;

public class GoblinPathFollower : MonoBehaviour
{
    [Header("Path Settings")]
    public Transform pathParent; // assigned at runtime by RespawnManager
    public float moveSpeed = 2f;
    public float reachThreshold = 0.2f;

    [Header("Animator Settings")]
    public Animator animator;
    public string walkParam = "isWalking";

    [Header("Spawn Offset")]
    public float enemyHeightOffset = -0.5f;

    private Transform[] waypoints;
    private int currentWaypoint = 0;

    void Start()
    {
        // If pathParent already assigned in inspector (scene-instance), initialize
        if (pathParent != null && (waypoints == null || waypoints.Length == 0))
            InitializePath();
    }

    public void InitializePath()
    {
        if (pathParent != null && pathParent.childCount > 0)
        {
            waypoints = new Transform[pathParent.childCount];
            for (int i = 0; i < pathParent.childCount; i++)
                waypoints[i] = pathParent.GetChild(i);

            if (animator != null) animator.SetBool(walkParam, true);

            Vector3 spawnOffset = new Vector3(0f, enemyHeightOffset, 0f);
            transform.position = waypoints[0].position + spawnOffset;

            currentWaypoint = 1;
        }
        else
        {
            Debug.LogWarning($"{name}: No waypoints assigned or pathParent has no children.");
        }
    }

    void Update()
    {
        if (waypoints == null || currentWaypoint >= waypoints.Length) return;

        Vector3 targetPos = waypoints[currentWaypoint].position;

        Vector3 direction = (targetPos - transform.position);
        direction.z = 0f;
        float distThisFrame = moveSpeed * Time.deltaTime;

        if (direction.magnitude <= distThisFrame)
        {
            // reached this waypoint exactly
            transform.position = new Vector3(targetPos.x, targetPos.y, 0f);
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
            {
                if (animator != null) animator.SetBool(walkParam, false);
                Destroy(gameObject);
            }
            return;
        }

        // move
        transform.Translate(direction.normalized * distThisFrame, Space.World);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

        // flip sprite
        if (direction.x > 0f) transform.localScale = new Vector3(1f, 1f, 1f);
        else if (direction.x < 0f) transform.localScale = new Vector3(-1f, 1f, 1f);
    }

    // Called by RespawnManager after assigning pathParent to freshly instantiated enemy
    public void RespawnAtStart()
    {
        // reinitialize array & position
        InitializePath();
    }
}
