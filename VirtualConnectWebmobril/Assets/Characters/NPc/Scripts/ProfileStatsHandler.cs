using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Profiling;
using UnityEngine.Serialization;

public class ProfileStatsHandler : MonoBehaviour
{
    public float timer, refresh, avgFramerate;
    string display = "{0} FPS";
    [SerializeField]private TextMeshProUGUI FPS_Text;
 
    private void Update()
    {
        //Change smoothDeltaTime to deltaTime or fixedDeltaTime to see the difference
        float timelapse = Time.deltaTime;
        timer = timer <= 0 ? refresh : timer -= timelapse;
 
        if(timer <= 0) avgFramerate = (int) (1f / timelapse);
        FPS_Text.text = string.Format(display,avgFramerate.ToString());
    }
}
