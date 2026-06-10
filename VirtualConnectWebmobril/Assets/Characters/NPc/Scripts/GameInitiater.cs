using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitiater : MonoBehaviour
{

    [SerializeField] private GameObject BGScore, AudioHandler;
    private void Start()
    {
        var ahdlr = FindObjectOfType<AudioHandler>();
        if (ahdlr == null)
        {
            var audioHandler = Instantiate(AudioHandler);
            DontDestroyOnLoad(audioHandler);    
        }
        
        var asrc = FindObjectOfType<AudioSource>();
        if (asrc == null)
        {
            var bgScore = Instantiate(BGScore);
            DontDestroyOnLoad(bgScore);
            AudioManager.PlaySound(Sounds.Music , bgScore.GetComponent<AudioSource>());
        }
    }
}
