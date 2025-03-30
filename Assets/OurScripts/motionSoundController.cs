using UnityEngine;

public class MotionSoundController : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource tiltAudio;
    public AudioSource wobbleAudio;
    public AudioSource twistAudio;
    public AudioSource lagAudio;

    [Header("Developer Settings")]
    public bool disableAllSounds = false;

    public void SetTilt(bool enabled)
    {
        ToggleAudio(tiltAudio, enabled);
    }

    public void SetWobble(bool enabled)
    {
        ToggleAudio(wobbleAudio, enabled);
    }

    public void SetTwist(bool enabled)
    {
        ToggleAudio(twistAudio, enabled);
    }

    public void SetLag(bool enabled)
    {
        ToggleAudio(lagAudio, enabled);
    }

    private void ToggleAudio(AudioSource source, bool state)
    {
        if (source == null) return;

        if (disableAllSounds)
        {
            if (source.isPlaying)
                source.Stop();
            return;
        }

        if (state && !source.isPlaying)
            source.Play();
        else if (!state && source.isPlaying)
            source.Stop();
    }

    void Start()
    {
        if (disableAllSounds)
        {
            if (tiltAudio != null) tiltAudio.Stop();
            if (wobbleAudio != null) wobbleAudio.Stop();
            if (twistAudio != null) twistAudio.Stop();
            if (lagAudio != null) lagAudio.Stop();
        }
    }
}
