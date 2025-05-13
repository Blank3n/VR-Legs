using UnityEngine;
using UnityEngine.InputSystem;

public class VRScore : MonoBehaviour
{
    public int maxScore = 100;
    private int currentScore;

    [Header("Input")]
    public InputActionReference vrLegsPressAction; // ← Dra in din InputAction här i Inspector

    private void OnEnable()
    {
        if (vrLegsPressAction != null)
            vrLegsPressAction.action.Enable(); // Aktivera action om den inte redan är aktiverad
            vrLegsPressAction.action.performed += OnLegsPress;
    }

    private void OnDisable()
    {
        if (vrLegsPressAction != null)
            vrLegsPressAction.action.performed -= OnLegsPress;
    }

    void Start()
    {
        currentScore = maxScore;
    }

    private void OnLegsPress(InputAction.CallbackContext context)
    {
        currentScore = Mathf.Max(0, currentScore - 1);
        Debug.Log("B-knapp (VRLegsPress) tryckt. Nuvarande poäng: " + currentScore);
    }

    public int GetFinalScore()
    {
        return currentScore;
    }
}
