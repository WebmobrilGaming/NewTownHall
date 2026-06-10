using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[Serializable]
public class AudioClips
{
    public Sounds sound;
    public AudioClip audioClip;
}
public class AudioHandler : MonoBehaviour
{

    public static AudioHandler instance {get;  private set; }
    [NonReorderable]public List<AudioClips> AudioClips = new List<AudioClips>();
    [SerializeField]private List<AudioSource> AudioSourcesInScene = new List<AudioSource>();
    [FormerlySerializedAs("Mute")] public bool mute;

    private void Awake()
    {
        instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        AudioSourcesInScene.Clear();
        Debug.Log("AudioHandler OnSceneLoaded");
        foreach (var audioSource in FindObjectsOfType<AudioSource>())
        {
            AudioSourcesInScene.Add(audioSource);
        }
        if(AudioManager.IsMuted) AudioManager.Mute();
    }

    public List<AudioSource> GetAllAudioScources()
    {
        if(AudioSourcesInScene != null) return AudioSourcesInScene;
        else
        {
            Debug.LogError("Audio sources is null");
            return null;
        }
    }

    public AudioClip GetAudioClip(Sounds sounds)
    {
        foreach (var clips in AudioClips)
        {
            if (clips.sound == sounds) return clips.audioClip;
        }
        Debug.LogError("No Such sound available" + sounds.ToString());
        return null;
    }

}
