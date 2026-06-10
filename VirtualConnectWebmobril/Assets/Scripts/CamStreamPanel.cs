using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using VirtualConnectUtils.Act;


public class CamStreamPanel : MonoBehaviour
{
   [SerializeField] RawImage videoImage;

    private void OnEnable()
    {
        Actions.SetVideoTexture += UpdateVideoTexture;
    }

    private void OnDisable()
    {
        Actions.SetVideoTexture -= UpdateVideoTexture;
    }

    void UpdateVideoTexture(Texture newTexture)
    {
        videoImage.gameObject.SetActive(true);
        videoImage.texture = newTexture;
    }
}
