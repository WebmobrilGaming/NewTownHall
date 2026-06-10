using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtualConnect
{
    public class AgoraData : ScriptableObject
    {
        public string playerID = String.Empty;
        public string appID = String.Empty;
        public string tokenID = String.Empty;
        public string channelName = String.Empty;
    }
}
