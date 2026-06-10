using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using VirtualConnectUtils.Act;

[RequireComponent(typeof(RawImage))]
public class VideoImageObj : MonoBehaviour
{
    [SerializeField] RawImage videoImage;

    [SerializeField] Button videoImgButton;

    private void OnEnable()
    {
        videoImgButton.onClick.AddListener(VideoImgButtonClicked);
    }

    private void OnDisable()
    {
        videoImgButton.onClick.RemoveListener(VideoImgButtonClicked);
    }


    void VideoImgButtonClicked()
    {
        Debug.Log("Video Image Button Clicked!");
        Texture texture =  videoImage.texture ;

        Actions.SetVideoTexture?.Invoke(texture);
    }
}
