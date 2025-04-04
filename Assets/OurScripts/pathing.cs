using System.Collections.Generic;
using UnityEngine;

public class ModularPathSkateboard : MonoBehaviour
{
    [Header("Path settings")]
    public Transform[] pathSegments;
    [HideInInspector] public float currentSpeed = 5f;
    [HideInInspector] public float currentRotationSpeed = 2f;
    public bool loop = true;

    private List<Transform> allWaypoints = new List<Transform>();
    private int currentIndex = 0;

    void Start()
    {
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

        Vector3 direction = (target.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * currentRotationSpeed);
        }

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
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
    }
}
