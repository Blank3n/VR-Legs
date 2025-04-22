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

    [Header("Sound Control")]
    public MotionSoundController soundController;

    private int currentWaypointIndex = 0;
    private int currentPathIndex = 0;

    // Farthantering
    private float currentSpeed = 0f;
    private float startSpeed = 0f;
    private float targetSpeed = 0f;
    private float accelerationDuration = 0f;
    private float speedTimer = 0f;

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

        // 🟢 Farthantering med acceleration/inbromsning
        if (accelerationDuration > 0f)
        {
            speedTimer += Time.deltaTime;
            float t = Mathf.Clamp01(speedTimer / accelerationDuration);
            currentSpeed = Mathf.Lerp(startSpeed, targetSpeed, t);
        }
        else
        {
            currentSpeed = targetSpeed;
        }

        if (pathingScript != null)
        {
            pathingScript.currentSpeed = currentSpeed;
        }

        // 🟡 Waypoint-hantering
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
            else
            {
                currentPathIndex = 0;
            }
        }
    }

    void ApplySettingsForCurrentPath()
    {
        var settings = pathSegments[currentPathIndex];

        // Motion scripts
        if (tiltScript != null)
        {
            tiltScript.enableTilt = settings.enableTilt;
            tiltScript.tiltStrength = settings.tiltStrength;
            tiltScript.tiltSpeed = settings.tiltSpeed;
        }

        if (wobbleScript != null)
        {
            wobbleScript.enableWobble = settings.enableWobble;
            wobbleScript.wobbleStrength = settings.wobbleStrength;
            wobbleScript.wobbleSpeed = settings.wobbleSpeed;
        }

        if (twistScript != null)
        {
            twistScript.enableTwist = settings.enableTwist;
            twistScript.twistStrength = settings.twistStrength;
            twistScript.twistDuration = settings.twistDuration;
            twistScript.twistInterval = settings.twistInterval;
        }

        if (lagScript != null)
        {
            lagScript.enableLag = settings.enableMotionLag;
            lagScript.targetFPS = settings.targetFPS;
            lagScript.timeBetweenJumps = settings.timeBetweenJumps;
        }

        if (swingScript != null)
        {
            swingScript.enableSwing = settings.enableSwing;
            swingScript.length = settings.length;
            swingScript.gravity = settings.gravity;
            swingScript.maxPushForce = settings.maxPushForce;
        }

        if (soundController != null)
        {
            soundController.SetTilt(settings.enableTilt);
            soundController.SetWobble(settings.enableWobble);
            soundController.SetTwist(settings.enableTwist);
            soundController.SetLag(settings.enableMotionLag);
        }

        // 🟢 Uppdatera fartkurva
        startSpeed = settings.startSpeed;
        targetSpeed = settings.targetSpeed;
        accelerationDuration = settings.accelerationDuration;
        speedTimer = 0f;
        currentSpeed = startSpeed;

        if (pathingScript != null)
        {
            pathingScript.currentRotationSpeed = settings.rotationSpeed;
        }

        Debug.Log($"▶️ Segment {currentPathIndex} | {startSpeed} → {targetSpeed} på {accelerationDuration}s");
    }
}
 