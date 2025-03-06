using UnityEngine;

public class SwingScore : MonoBehaviour
{
    private float previousRotation = 0f;
    private int score = 0;

    void Update()
    {
        // Hämtar gungans rotation runt Z-axeln
        float currentRotation = transform.eulerAngles.z;

        // Kolla om rotationen har gått ett helt varv (360 grader)
        if (previousRotation > 270f && currentRotation < 90f)
        {
            score++;
            Debug.Log("Slått runt! Poäng: " + score);
        }

        // Uppdatera föregående rotation
        previousRotation = currentRotation;
    }
}