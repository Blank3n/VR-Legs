using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;  // Required for UI Text

public class SwingRotationCounter : MonoBehaviour
{
    public TextMeshPro rotationText; // Drag the UI Text here

    private int rotationCount = 0;
    private float previousAngle = 0f;
    private float totalRotation = 0f;

    void Start()
    {
        previousAngle = transform.eulerAngles.z; // Track initial rotation
    }

    void Update()
    {
        float currentAngle = transform.eulerAngles.z;  // Adjust if rotating on another axis
        float deltaAngle = Mathf.DeltaAngle(previousAngle, currentAngle);
        totalRotation += deltaAngle;

        // Detect a full 360° rotation
        if (Mathf.Abs(totalRotation) >= 360f)
        {
            rotationCount++;
            totalRotation = 0f; // Reset rotation tracking
        }

        previousAngle = currentAngle;

        // Update the UI text
        if (rotationText != null)
        {
            rotationText.text = "" + rotationCount;
        }
    }
}