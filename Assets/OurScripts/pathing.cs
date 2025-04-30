using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class ModularPathSkateboard : MonoBehaviour
{
    [Header("Path settings")]
    public Transform[] pathSegments;
    [HideInInspector] public float currentSpeed = 5f;
    [HideInInspector] public float currentRotationSpeed = 2f;
    public bool loop = true;

    [Header("Waypoint Triggers")]
    public WaypointTrigger[] waypointTriggers;

    [Header("Termination Waypoints")]
    public Transform[] terminateWaypoints; // Waypoints that end the game

    private List<Transform> allWaypoints = new List<Transform>();
    private int currentIndex = 0;

    [System.Serializable]
    public class WaypointTrigger
    {
        public Transform triggerWaypoint;
        public GameObject objectToActivate;
        [HideInInspector] public bool triggered = false;
    }

    void Start()
    {
        // Initialize all waypoints
        foreach (Transform segment in pathSegments)
        {
            foreach (Transform waypoint in segment)
            {
                allWaypoints.Add(waypoint);
            }
        }

        if (allWaypoints.Count > 0)
        {
            transform.position = allWaypoints[0].position;
        }
    }

    void Update()
    {
        if (allWaypoints.Count == 0) return;

        Transform target = allWaypoints[currentIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, currentSpeed * Time.deltaTime);

        // Handle rotation
        Vector3 direction = (target.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * currentRotationSpeed);
        }

        // Check triggers when reaching a waypoint
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            CheckWaypointTriggers(target);
            CheckTerminationWaypoints(target);
            
            currentIndex++;
            if (currentIndex >= allWaypoints.Count)
            {
                if (loop)
                {
                    currentIndex = 0;
                }
                else
                {
                    enabled = false;
                }
            }
        }
    }

    void CheckWaypointTriggers(Transform reachedWaypoint)
    {
        if (waypointTriggers == null) return;

        foreach (WaypointTrigger trigger in waypointTriggers)
        {
            if (!trigger.triggered && trigger.triggerWaypoint == reachedWaypoint)
            {
                trigger.triggered = true;
                if (trigger.objectToActivate != null)
                {
                    trigger.objectToActivate.SetActive(true);
                    
                    ModularPathSkateboard otherSkateboard = trigger.objectToActivate.GetComponent<ModularPathSkateboard>();
                    if (otherSkateboard != null)
                    {
                        otherSkateboard.currentSpeed = this.currentSpeed*2;
                        otherSkateboard.currentRotationSpeed = this.currentRotationSpeed;
                    }
                }
            }
        }
    }

    void CheckTerminationWaypoints(Transform reachedWaypoint)
    {
        if (terminateWaypoints == null) return;

        foreach (Transform terminatePoint in terminateWaypoints)
        {
            if (terminatePoint == reachedWaypoint)
            {
                TerminateGame();
                return; // Exit after finding first matching termination point
            }
        }
    }

    void TerminateGame()
    {
        Debug.Log("Game terminated - reached termination waypoint");
        // Example termination actions (choose one):
        // 1. Reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        // 2. Or go to game over screen
        // SceneManager.LoadScene("GameOverScene");
        
        // 3. Or quit application (for build)
        // Application.Quit();
        
        // 4. Or simply stop movement
        // enabled = false;
    }

    void OnDrawGizmos()
    {
        if (pathSegments == null || pathSegments.Length == 0) return;

        List<Transform> waypoints = new List<Transform>();
        foreach (Transform segment in pathSegments)
        {
            foreach (Transform waypoint in segment)
            {
                waypoints.Add(waypoint);
            }
        }

        // Draw path
        Gizmos.color = Color.red;
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            if (waypoints[i] != null && waypoints[i + 1] != null)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
        }

        if (loop && waypoints.Count > 1)
        {
            Gizmos.DrawLine(waypoints[waypoints.Count - 1].position, waypoints[0].position);
        }

        // Draw trigger waypoints in green
        if (waypointTriggers != null)
        {
            Gizmos.color = Color.green;
            foreach (var trigger in waypointTriggers)
            {
                if (trigger.triggerWaypoint != null)
                {
                    Gizmos.DrawSphere(trigger.triggerWaypoint.position, 0.3f);
                }
            }
        }

        // Draw termination waypoints in black
        if (terminateWaypoints != null)
        {
            Gizmos.color = Color.black;
            foreach (var terminatePoint in terminateWaypoints)
            {
                if (terminatePoint != null)
                {
                    Gizmos.DrawCube(terminatePoint.position, Vector3.one * 0.5f);
                }
            }
        }
    }
}