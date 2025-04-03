using UnityEngine;

[System.Serializable]
public class PathSegment
{
    public Transform segment;

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

    [Header("Motion Swing-inställningar")]
    public bool enableSwing;
    public float length = 2f; // Length of the swing
    public float gravity = 9.81f; // Gravity effect
    public float maxPushForce = 1f; // Max force applied when pushing

}
