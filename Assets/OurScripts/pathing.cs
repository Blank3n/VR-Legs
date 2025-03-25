using System.Collections.Generic;
using UnityEngine;

public class ModularPathSkateboard : MonoBehaviour
{
    [Header("Path settings")]
    public Transform[] pathSegments; // Dra in path-segmenten i rätt ordning
    public float speed = 5f;
    public float rotationSpeed = 5f;
    public bool loop = true;

    private List<Transform> allWaypoints = new List<Transform>();
    private int currentIndex = 0;

    void Start()
    {
        // Sätt ihop alla waypoints från varje path-segment
        foreach (Transform segment in pathSegments)
        {
            foreach (Transform waypoint in segment)
            {
                allWaypoints.Add(waypoint);
            }
        }

        // Startposition (om det finns waypoints)
        if (allWaypoints.Count > 0)
        {
            transform.position = allWaypoints[0].position;
        }
    }

    void Update()
    {
        if (allWaypoints.Count == 0) return;

        Transform target = allWaypoints[currentIndex];

        // Rör sig mot target waypoint
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Roterar mjukt i rörelseriktningen
        Vector3 direction = (target.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        // Gå till nästa waypoint om nära nog
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            currentIndex++;
            if (currentIndex >= allWaypoints.Count)
            {
                if (loop)
                    currentIndex = 0;
                else
                    enabled = false; // Stanna när sista waypoint är nådd
            }
        }
    }
}