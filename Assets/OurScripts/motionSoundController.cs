using UnityEngine;

public class MotionSoundController : MonoBehaviour
{
    public AudioSource tiltAudio;
    public AudioSource wobbleAudio;
    public AudioSource twistAudio;
    public AudioSource lagAudio;

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

        if (state && !source.isPlaying)
            source.Play();
        else if (!state && source.isPlaying)
            source.Stop();
    }
}
