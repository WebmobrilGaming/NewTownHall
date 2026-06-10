using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public enum Sounds
{
    Music,
    PlayeFootSteos
}
public static class AudioManager
{
    private static bool isMuted;
    public static bool IsMuted => isMuted;
    public static void PlaySound(Sounds sounds , AudioSource audioSource)
    {
        audioSource.clip = AudioHandler.instance.GetAudioClip(sounds);
        audioSource.Play();
    }

    public static void StopSound(AudioSource audioSource)
    {
        audioSource.Stop();
    }

    public static void Mute()
    {
        HandleMute(true);
    }
    
    public static void UnMute()
    {
        HandleMute(false);
    }

    static void HandleMute(bool state)
    {
        var audioSources = AudioHandler.instance.GetAllAudioScources();
        foreach (var audio in audioSources)
        {
            audio.mute = state;
        }
        isMuted = state;
    }
}
