using UnityEngine;

[System.Serializable]
public class PathSegment
{
    public Transform segment;

    [Header("Speed Transition")]
    public float startSpeed = 0f;
    public float targetSpeed = 5f;
    public float accelerationDuration = 2f;

    [Header("Tilt-inställningar")]
    public bool enableTilt;
    public float tiltStrength = 10f;
    public float tiltSpeed = 2f;

    [Header("Wobble-inställningar")]
    public bool enableWobble;
    public float wobbleStrength = 5f;
    public float wobbleSpeed = 5f;

    [Header("Twist-inställningar")]
    public bool enableTwist;
    public float twistStrength = 15f;
    public float twistDuration = 1f;
    public float twistInterval = 4f;

    [Header("Motion Lag-inställningar")]
    public bool enableMotionLag;
    public int targetFPS = 15;
    public float timeBetweenJumps = 5f;

    [Header("Swing-inställningar")]
    public bool enableSwing;
    public float length = 2f;
    public float gravity = 9.81f;
    public float maxPushForce = 1f;

    [Header("Swing-inställningar")]
    public bool enableSpin = true;
    public float rotationSpeed = 0f;
}
 