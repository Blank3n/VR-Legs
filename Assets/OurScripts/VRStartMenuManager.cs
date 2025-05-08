using UnityEngine;
using UnityEngine.UI;


public class VRStartMenuManager : MonoBehaviour
{
    public GameObject startMenuCanvas;              // Assign your canvas here
    public MonoBehaviour[] experienceScripts;       // Drag any movement scripts here
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor leftRayInteractor;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rightRayInteractor;

    private void Start()
    {
        // Disable logic scripts until player starts
        foreach (var script in experienceScripts)
        {
            script.enabled = false;
        }

        // Enable UI interaction raycasters
        if (leftRayInteractor != null)
            leftRayInteractor.gameObject.SetActive(true);
        if (rightRayInteractor != null)
            rightRayInteractor.gameObject.SetActive(true);

        startMenuCanvas.SetActive(true);
    }

    public void OnPlayButtonPressed()
    {
        // Enable logic scripts
        foreach (var script in experienceScripts)
        {
            script.enabled = true;
        }

        // Optionally hide menu
        startMenuCanvas.SetActive(false);
    }
}
