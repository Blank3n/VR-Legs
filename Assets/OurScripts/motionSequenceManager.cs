using System.Collections.Generic;
using UnityEngine;

public class MotionSequenceManager : MonoBehaviour
{
    [Header("Pathing")]
    public ModularPathSkateboard pathingScript;

    [Header("Waypoint Path (alla i ordning)")]
    public Transform[] waypoints;

    [Header("Path Segment Settings")]
    public List<PathSegment> pathSegments;

    [Header("Motion Effects")]
    public motionTilt tiltScript;
    public motionWobble wobbleScript;
    public motionTwist twistScript;
    public motionLag lagScript;
    public motionSwing swingScript;
    public motionSpin spinScript;

    [Header("Sound Control")]
    public MotionSoundController soundController;

    private int currentWaypointIndex = 0;
    private int currentPathIndex = 0;

    void Start()
    {
        if (pathSegments == null || pathSegments.Count == 0)
        {
            Debug.LogError("❌ PathSegments saknas.");
            enabled = false;
            return;
        }

        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("❌ Waypoints saknas.");
            enabled = false;
            return;
        }

        ApplySettingsForCurrentPath();
    }

    void Update()
    {
        if (currentWaypointIndex >= waypoints.Length) return;

        Transform currentWaypoint = waypoints[currentWaypointIndex];
        float distance = Vector3.Distance(transform.position, currentWaypoint.position);

        if (distance < 0.2f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
                currentPathIndex = 0;
                ApplySettingsForCurrentPath();
                return;
            }

            if (currentPathIndex + 1 < pathSegments.Count)
            {
                Transform nextPath = pathSegments[currentPathIndex + 1].segment;
                if (currentWaypoint.parent == nextPath)
                {
                    currentPathIndex++;
                    ApplySettingsForCurrentPath();
                }
            }
        }
    }

    void ApplySettingsForCurrentPath()
    {
        var settings = pathSegments[currentPathIndex];

        // Tilt
        if (tiltScript != null)
        {
            tiltScript.enableTilt = settings.enableTilt;
            tiltScript.tiltStrength = settings.tiltStrength;
            tiltScript.tiltSpeed = settings.tiltSpeed;
        }

        // Wobble
        if (wobbleScript != null)
        {
            wobbleScript.enableWobble = settings.enableWobble;
            wobbleScript.wobbleStrength = settings.wobbleStrength;
            wobbleScript.wobbleSpeed = settings.wobbleSpeed;
        }

        // Twist
        if (twistScript != null)
        {
            twistScript.enableTwist = settings.enableTwist;
            twistScript.twistStrength = settings.twistStrength;
            twistScript.twistDuration = settings.twistDuration;
            twistScript.twistInterval = settings.twistInterval;
        }

        // Motion Lag
        if (lagScript != null)
        {
            lagScript.enableLag = settings.enableMotionLag;
            lagScript.targetFPS = settings.targetFPS;
            lagScript.timeBetweenJumps = settings.timeBetweenJumps;
        }

        // Swing
        if (swingScript != null)
        {
            swingScript.enableSwing = settings.enableSwing;
            swingScript.length = settings.length;
            swingScript.gravity = settings.gravity;
            swingScript.maxPushForce = settings.maxPushForce;
        }

        // Spin
        if (spinScript != null)
        {
            spinScript.enableSpin = settings.enableSpin;
            spinScript.rotSpeed = settings.rotSpeed;
        }

        // Sound
        if (soundController != null)
        {
            soundController.SetTilt(settings.enableTilt);
            soundController.SetWobble(settings.enableWobble);
            soundController.SetTwist(settings.enableTwist);
            soundController.SetLag(settings.enableMotionLag);
        }

        // Path Speed & Rotation
        if (pathingScript != null)
        {
            pathingScript.currentSpeed = settings.speed;
            pathingScript.currentRotationSpeed = settings.rotationSpeed;
        }

        Debug.Log($"▶️ Bytte till path: {settings.segment.name} | Speed: {settings.speed}, Rotation: {settings.rotationSpeed}");
    }
}
