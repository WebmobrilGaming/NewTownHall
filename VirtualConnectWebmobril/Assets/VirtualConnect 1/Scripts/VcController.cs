using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Agora.Rtc;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Video;


namespace VirtualConnect
{
    public class VcController : MonoBehaviour
    {
        [SerializeField] private AppIdInput appData;
        private uint playerID;
        public void SetPlayerID(uint value) => playerID = value;
        
        internal IRtcEngine RtcEngine;

        [SerializeField] private GameObject menuPanel;
        [SerializeField] private GameObject videoPanel;

        [SerializeField] private Button joinBtn;
        [SerializeField] private Button remoteBtn;
        [SerializeField] private Button closeBtn;
        
        [SerializeField] private Button screenShareStartBtn;
        [SerializeField] private Button screenShareStopBtn;
        
        [SerializeField] private Button audioMuteBtn;
        [SerializeField] private Button audioUnmuteBtn;
        
        [SerializeField] private Button micrphoneMuteBtn;
        [SerializeField] private Button micrphoneUnmuteBtn;

        [SerializeField] private GameObject videoImage;
        [SerializeField] private RectTransform videoImageParent;
        private Dictionary<uint, GameObject> videoScreens = new Dictionary<uint, GameObject>();
        private bool[] pos = new bool[5];
        private Dictionary<uint, int> posData = new Dictionary<uint, int>();

        private bool joinedAsAudience = false;
        public bool GetJoinedAsAudience => joinedAsAudience;

        public static Action OnplayerJoin;

        public Transform[] spwanPoints;
        public int index = 0;

        //private Transform oldPos;
        //[SerializeField] private Transform newPOs;
        [SerializeField] private RectTransform newSize;
        [SerializeField] private RectTransform oldSize;

        [SerializeField] private VideoClip[] videoClips;
        [SerializeField] private VideoPlayer vPlayer;
        [SerializeField] private GameObject videoPanelMP4;
       // [SerializeField] private GameObject videoControlPanel;
        [SerializeField] private Button play1;
        [SerializeField] private Button play2;
        [SerializeField] private Button play3;
        [SerializeField] private Button play4;
        [SerializeField] private Button closeVideo;
        [SerializeField] private Button rotateBtn;
        private int videoPlayingIndex = 0;

        //public void EnableVideoControlPanel() =>;

        private bool isScreenSharing = false;
        private bool isScreenEnlarged = false;

        [SerializeField] private GameObject newSurefaceObject;
        [SerializeField] private VideoSurface newSureface;

        [SerializeField] private FlipButton[] flipButtons;
        [SerializeField] private GameObject roatateIcons;
        private bool isIconsRotated;

        private uint tmpId;
        
        [SerializeField] private Button display1;
        [SerializeField] private Button display2;
        [SerializeField] private GameObject displaySelectPopUp;
        private int selectedDisplay = 0;

        [SerializeField] private TMP_InputField tokenInput;
        [SerializeField] private Button submitBtn;
        [SerializeField] private GameObject inputFiledPopUp;

        private void Start()
        {
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
            menuPanel.SetActive(true);
            videoPanel.SetActive(false);
            
            AddListeners();


            SetupVideoSDKEngine();
        }

        private void AddListeners()
        {
            joinBtn.onClick.AddListener(() => { Join(); });

            remoteBtn.onClick.AddListener(() => { JoinAsAudience(); });

            closeBtn.onClick.AddListener(() => { Application.Quit(); });

            screenShareStartBtn.onClick.AddListener(() =>
            {
                displaySelectPopUp.SetActive(true);
                //StartScreenShare();
            });

            screenShareStopBtn.onClick.AddListener(() =>
            {
                ShareScreen(false, 0);
                screenShareStartBtn.gameObject.SetActive(true);
                screenShareStopBtn.gameObject.SetActive(false);
                isScreenSharing = false;
            });
            
            audioMuteBtn.onClick.AddListener(() => { Mute(true); });
            
            audioUnmuteBtn.onClick.AddListener(() => { Mute(false); });
            
            micrphoneMuteBtn.onClick.AddListener(() => { MuteUnmuteStream(true); });
            
            micrphoneUnmuteBtn.onClick.AddListener(() => { MuteUnmuteStream(false); });
            
            
            play1.onClick.AddListener(() =>
            {
                videoPlayingIndex = 0;
                PlayVideo();
            });
            
            play2.onClick.AddListener(() =>
            {
                videoPlayingIndex = 1;
                PlayVideo();
            });
            
            play3.onClick.AddListener(() =>
            {
                videoPlayingIndex = 2;
                PlayVideo();
            });
            
            play4.onClick.AddListener(() =>
            {
                videoPlayingIndex = 3;
                PlayVideo();
            });
            
            closeVideo.onClick.AddListener(() =>
            {
                StopVideo();
            });
            
            rotateBtn.onClick.AddListener(() =>
            {
                isIconsRotated = !isIconsRotated;
                roatateIcons.SetActive(isIconsRotated);
            });
            
            display1.onClick.AddListener(() =>
            {
                displaySelectPopUp.SetActive(false);
                selectedDisplay = 0;
                StartScreenShare();
            });
            
            display2.onClick.AddListener(() =>
            {
                displaySelectPopUp.SetActive(false);
                selectedDisplay = 1;
                StartScreenShare();
            });
            
            submitBtn.onClick.AddListener(() =>
            {
                appData.token = tokenInput.text;
                inputFiledPopUp.SetActive(false);
            });
            
            FlipButton.OnRotateClick += OnRotateClick;
        }

        public void StopSharing()
        {
            ShareScreen(false, 0);
                screenShareStartBtn.gameObject.SetActive(true);
                screenShareStopBtn.gameObject.SetActive(false);
                isScreenSharing = false;
        }

        private void StartScreenShare()
        {
            RtcEngine.StopScreenCapture();
            ShareScreen(true, 0);
            screenShareStartBtn.gameObject.SetActive(false);
            screenShareStopBtn.gameObject.SetActive(true);
            isScreenSharing = true;
        }

        private void OnRotateClick(uint obj)
        {
            RotatePanel(90, obj);
        }

        private void PlayVideo()
        {
            vPlayer.clip = videoClips[videoPlayingIndex];
            vPlayer.Play();
            videoPanelMP4.SetActive(true);
            closeVideo.gameObject.SetActive(true);
        }

        private void StopVideo()
        {
            vPlayer.Pause();
            videoPanelMP4.SetActive(false);
            closeVideo.gameObject.SetActive(false);
        }

        private void Mute(bool value)
        {
            if (value)
            {
                //Mute the audio and activate unmute button
                RtcEngine.MuteAllRemoteAudioStreams(true);
                //RtcEngine.MuteLocalAudioStream(true);
                
                audioMuteBtn.gameObject.SetActive(false);
                audioUnmuteBtn.gameObject.SetActive(true);
            }
            else
            {
                //UnMute the audio and activate mute button
                RtcEngine.MuteAllRemoteAudioStreams(false);
                //RtcEngine.MuteLocalAudioStream(false);
                
                audioMuteBtn.gameObject.SetActive(true);
                audioUnmuteBtn.gameObject.SetActive(false);
            }
        }
        
        private void MuteUnmuteStream(bool value)
        {
            if (value)
            {
                //Play the video and activate pause button
                RtcEngine.MuteLocalAudioStream(true);
                micrphoneMuteBtn.gameObject.SetActive(false);
                micrphoneUnmuteBtn.gameObject.SetActive(true);
            }
            else
            {
                //Pause the video and activate Play button
                RtcEngine.MuteLocalAudioStream(false);
                micrphoneMuteBtn.gameObject.SetActive(true);
                micrphoneUnmuteBtn.gameObject.SetActive(false);
            }
        }

        private void Clicked(uint _id, bool value)
        {
            if (isScreenEnlarged)
            {
                if (tmpId != _id)
                {
                    return;
                }
            }
            if (value)
            {
                newSurefaceObject.SetActive(true);
                ButtonData btn = videoScreens[_id].GetComponent<ButtonData>();
                ButtonData btn1 = newSurefaceObject.GetComponent<ButtonData>();
                btn1.id = btn.id;
                btn1.value = !btn.value;
                videoScreens[_id].gameObject.SetActive(false);

                tmpId = _id;

                VideoSurface vs = Instantiate(newSureface, newSurefaceObject.transform);
                vs.gameObject.SetActive(true);

                if (_id == 0)
                {
                    vs.SetForUser(_id, "Test", isScreenSharing ? VIDEO_SOURCE_TYPE.VIDEO_SOURCE_SCREEN_PRIMARY : VIDEO_SOURCE_TYPE.VIDEO_SOURCE_CAMERA);
                    vs.SetEnable(true);
                }
                else
                {
                    vs.SetForUser(_id, "Test", VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
                    vs.SetEnable(true);
                }

                if (isScreenSharing && _id == 0)
                {
                    RotatePanel(90, _id);
                }
                else
                {
                    RotatePanel(-90, _id);
                }

                isScreenEnlarged = true;
            }
            else
            {
                newSurefaceObject.SetActive(false);
                videoScreens[_id].gameObject.SetActive(true);
                
                ButtonData btn = videoScreens[_id].GetComponent<ButtonData>();
                ButtonData btn1 = newSurefaceObject.GetComponent<ButtonData>();
                btn.value = !btn1.value;

                if (_id == 0)
                {
                    videoScreens[_id].transform.GetChild(0).GetComponent<VideoSurface>().SetForUser(0, "Test", isScreenSharing ? VIDEO_SOURCE_TYPE.VIDEO_SOURCE_SCREEN_PRIMARY : VIDEO_SOURCE_TYPE.VIDEO_SOURCE_CAMERA);
                    videoScreens[_id].transform.GetChild(0).GetComponent<VideoSurface>().SetEnable(true);
                    RtcEngine.MuteRemoteVideoStream(_id, false);
                }
                else
                {
                    videoScreens[_id].transform.GetChild(0).GetComponent<VideoSurface>().SetForUser(_id, "Test", VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
                    videoScreens[_id].transform.GetChild(0).GetComponent<VideoSurface>().SetEnable(true);
                    RtcEngine.MuteRemoteVideoStream(_id, false);
                }
                isScreenEnlarged = false;
            }
            
            Debug.Log(_id+" - "+value);
        }
        
        private void ObjDisable (Transform value) {
            for (var i = 0; i < value.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        private void VideoConfigSet()
        {
            VideoEncoderConfiguration config = new VideoEncoderConfiguration();
            config.orientationMode = ORIENTATION_MODE.ORIENTATION_MODE_ADAPTIVE;
            config.degradationPreference = DEGRADATION_PREFERENCE.MAINTAIN_QUALITY;
            RtcEngine.SetVideoEncoderConfiguration(config);
        }

        private void RotatePanel(float value, uint _id)
        {
            if (isScreenEnlarged)
            {
                RectTransform rt = newSurefaceObject.GetComponent<RectTransform>();
                Vector3 rot = rt.eulerAngles;
                rt.eulerAngles = new Vector3(rot.x, rot.y + value, rot.z);
            }
            else
            {
                RectTransform rt = videoScreens[_id].transform.GetChild(0).GetComponent<RectTransform>();
                Vector3 rot = rt.eulerAngles;
                rt.eulerAngles = new Vector3(rot.x, rot.y + value, rot.z);
            }
        }

        private void OnEnable()
        {
            ButtonData.clicked += Clicked;
        }

        private void OnDisable()
        {
            FlipButton.OnRotateClick -= OnRotateClick;
            ButtonData.clicked -= Clicked;
            Leave();
        }
        
        private void SetupVideoSDKEngine()
        {
            RtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngine();
            RtcEngineContext context = new RtcEngineContext(appData.appID, 0,
                CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING, 
                AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT);
            RtcEngine.Initialize(context);
            
            InitEventHandler();
        }

        private void InitEventHandler()
        {
            UserEventHandler handler = new UserEventHandler(this);
            RtcEngine.InitEventHandler(handler);
        }

        public void Join()
        {
            joinedAsAudience = false;
            RtcEngine.EnableVideo();
            RtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
            RtcEngine.JoinChannel(appData.token, appData.channelName);
        }

        public void JoinAsAudience()
        {
            joinedAsAudience = true;
            RtcEngine.MuteLocalAudioStream(true);
            RtcEngine.EnableLocalAudio(false);
            RtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE);
            RtcEngine.JoinChannel(appData.token, appData.channelName);
            
            DisableButtons();
        }

        private void DisableButtons()
        {
            audioMuteBtn.gameObject.SetActive(false);
            audioUnmuteBtn.gameObject.SetActive(false);

            micrphoneMuteBtn.gameObject.SetActive(false);
            micrphoneUnmuteBtn.gameObject.SetActive(false);

            screenShareStartBtn.gameObject.SetActive(false);
            screenShareStopBtn.gameObject.SetActive(false);
        }

        public void Leave()
        {
            RtcEngine.LeaveChannel();
            RtcEngine.DisableVideo();
            
            if(GetJoinedAsAudience) return;
            RemoveUserFromVideoScreen(playerID);
        }

        public void AddUserInVideoScreen(uint id, string channelName, VIDEO_SOURCE_TYPE type)
        {
            if(videoScreens.ContainsKey(id)) return;
            int pos1 = -1;
            for (int i = 0; i < pos.Length; i++)
            {
                if (!pos[i])
                {
                    pos1 = i;
                    pos[i] = true;
                    posData.Add(id,i);
                    break;
                }
            }

            if (pos1 == -1) return;
            
            GameObject tmp = Instantiate(videoImage, spwanPoints[pos1]);
            tmp.transform.GetChild(0).GetComponent<VideoSurface>().SetForUser(id, channelName, type);
            tmp.transform.GetChild(0).GetComponent<VideoSurface>().SetEnable(true);
            RtcEngine.MuteRemoteVideoStream(id, false);
            videoScreens.Add(id,tmp);
            
            PlayerJoined();
            index++;

            tmp.GetComponent<ButtonData>().id = id;
            flipButtons[pos1].iD = id;
            flipButtons[pos1].gameObject.SetActive(true);
            VideoConfigSet();
        }

        public void PlayerJoined()
        {
            OnplayerJoin?.Invoke();
        }

        public void RemoveUserFromVideoScreen(uint id)
        {
            if(!videoScreens.ContainsKey(id)) return;
            VideoSurface tmp = videoScreens[id].transform.GetChild(0).GetComponent<VideoSurface>();
            tmp.SetEnable(false);
            GameObject tmp1 = videoScreens[id];
            videoScreens.Remove(id);
            Destroy(tmp1);

            pos[posData[id]] = false;
            posData.Remove(id); 
        }

        public void SetVideoPanelScreenActivate()
        {
            menuPanel.SetActive(false);
            videoPanel.SetActive(true);
        }
        
        private void ShareScreen(bool value,uint id)
        {
            if (value)
            {
                SIZE t = new SIZE(360,240);
                SIZE s = new SIZE(360,240);
                var info = RtcEngine.GetScreenCaptureSources(t, s, true);
                ulong dispId = info[selectedDisplay].sourceId;
                int sucess = RtcEngine.StartScreenCaptureByDisplayId((uint)dispId, default(Rectangle),
                    default(ScreenCaptureParameters));
                
                //RotatePanel(90, id);
            }
            else
            {
                RtcEngine.StopScreenCapture();
                //RotatePanel(-90, id);
            }
            
            SetScreenShareTrack(value,id);
        }
        

        private void SetScreenShareTrack(bool publishMediaPlayer, uint id)
        {
            ChannelMediaOptions channelOptions = new ChannelMediaOptions();
            channelOptions.publishScreenTrack.SetValue(publishMediaPlayer);
            channelOptions.publishScreenCaptureVideo.SetValue(publishMediaPlayer);
            //channelOptions.publishAudioTrack.SetValue(true);
            channelOptions.publishSecondaryScreenTrack.SetValue(publishMediaPlayer);
            channelOptions.publishCameraTrack.SetValue(!publishMediaPlayer);
            RtcEngine.UpdateChannelMediaOptions(channelOptions);

            if (publishMediaPlayer)
            {
                SetUpScreenShareSurface(id);
            }
            else
            {
                SetUpScreenShareStopSurface(id);
            }
        }
        
        private void SetUpScreenShareSurface(uint id)
        {
            if (isScreenEnlarged)
            {
                VideoSurface vs = Instantiate(newSureface, newSurefaceObject.transform);
                vs.gameObject.SetActive(true);
                vs.SetForUser(id, "Test", VIDEO_SOURCE_TYPE.VIDEO_SOURCE_SCREEN_PRIMARY);
                vs.SetEnable(true);
            }
            else
            {
                VideoSurface tmp = videoScreens[id].transform.GetChild(0).GetComponent<VideoSurface>();
                DestroyImmediate(tmp);
                VideoSurface tmp1 = videoScreens[id].transform.GetChild(0).AddComponent<VideoSurface>();
                tmp1.SetForUser(0,appData.channelName,VIDEO_SOURCE_TYPE.VIDEO_SOURCE_SCREEN_PRIMARY);
            }
        }
        
        private void SetUpScreenShareStopSurface(uint id)
        {
            if (isScreenEnlarged)
            {
                VideoSurface vs = Instantiate(newSureface, newSurefaceObject.transform);
                vs.gameObject.SetActive(true);
                vs.SetForUser(id, "Test", VIDEO_SOURCE_TYPE.VIDEO_SOURCE_CAMERA);
                vs.SetEnable(true);
            }
            else
            {
                VideoSurface tmp = videoScreens[id].transform.GetChild(0).GetComponent<VideoSurface>();
                DestroyImmediate(tmp);
                VideoSurface tmp1 = videoScreens[id].transform.GetChild(0).AddComponent<VideoSurface>();
                tmp1.SetForUser(0,appData.channelName,VIDEO_SOURCE_TYPE.VIDEO_SOURCE_CAMERA);
            }
        }

    }
    
    
    internal class UserEventHandler : IRtcEngineEventHandler
    {
        private readonly VcController _videoSample;

        internal UserEventHandler(VcController videoSample)
        {
            _videoSample = videoSample;
        }
        public override void OnJoinChannelSuccess(RtcConnection connection, int eslapsed)
        {
            //_videoSample.EnableVideoControlPanel();
            _videoSample.SetVideoPanelScreenActivate();
            _videoSample.SetPlayerID(0);
            if (_videoSample.GetJoinedAsAudience)
            {
                _videoSample.PlayerJoined();
                return;
            }
            Debug.Log("You joined channel: " +connection.channelId);
            _videoSample.AddUserInVideoScreen(0, connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_CAMERA);
            
        }
        
        public override void OnUserJoined(RtcConnection connection, uint uid, int elapsed)
        {
            _videoSample.AddUserInVideoScreen(uid, connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
            
            Debug.Log("Remote user joined channel: " +uid);
        }
        
        
        public override void OnUserOffline(RtcConnection connection, uint uid, USER_OFFLINE_REASON_TYPE reason)
        {
            //_videoSample.RemoteView.SetEnable(false);
            _videoSample.RemoveUserFromVideoScreen(uid);
        }

        public override void OnLeaveChannel(RtcConnection connection, RtcStats stats)
        {
            _videoSample.RemoveUserFromVideoScreen(connection.localUid);
        }
        
    }
}
