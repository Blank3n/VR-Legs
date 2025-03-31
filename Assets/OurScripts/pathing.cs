using System.Collections.Generic;
using UnityEngine;

public class ModularPathSkateboard : MonoBehaviour
{
    [Header("Path settings")]
    public Transform[] pathSegments;
    public float speed = 5f;
    public float rotationSpeed = 2f;
    public bool loop = true;
    
    [Header("Pendulum Settings")]
    public bool enableSwing = true;
    public float pendulumLength = 2f;
    public float pendulumGravity = 9.81f;
    public float maxPushForce = 1f;
    public float pendulumFriction = 0.999f;

    private List<Transform> allWaypoints = new List<Transform>();
    private int currentIndex = 0;
    private bool isSwinging = false;
    private float pendulumAngle = 0f;
    private float pendulumVelocity = 0f;
    private Quaternion initialRotation;

    void Start()
    {
        // Original waypoint setup
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
        if (isSwinging)
        {
            // Exact pendulum physics from SwingMotion
            float deltaTime = Time.deltaTime;
            
            // α = - (g / L) * sin(θ)
            float angularAcceleration = -(pendulumGravity / pendulumLength) * Mathf.Sin(pendulumAngle * Mathf.Deg2Rad);
            
            pendulumVelocity += angularAcceleration * deltaTime;
            pendulumVelocity *= pendulumFriction;
            pendulumAngle += pendulumVelocity * Mathf.Rad2Deg * deltaTime;
            
            transform.rotation = initialRotation * Quaternion.Euler(0, pendulumAngle, 0);
            return;
        }

        // Original movement code (unchanged)
        if (allWaypoints.Count == 0) return;

        Transform target = allWaypoints[currentIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        Vector3 direction = (target.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
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
                else if (enableSwing)
                {
                    StartPendulumSwing();
                }
                else
                {
                    enabled = false;
                }
            }
        }
    }

    void StartPendulumSwing()
    {
        isSwinging = true;
        initialRotation = transform.rotation;
        pendulumAngle = 0f;
        
        // Give a small initial push (like the VR controller would)
        pendulumVelocity = maxPushForce * 0.3f; 
    }

    // Original Gizmos code remains unchanged
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