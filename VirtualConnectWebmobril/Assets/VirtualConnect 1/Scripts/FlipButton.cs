
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VirtualConnect
{
    public class FlipButton : MonoBehaviour
    {
        public float rotateValue;
        public uint iD;
        public Button onClick;

        public static Action<uint> OnRotateClick;
        private void Start()
        {
            onClick.onClick.AddListener(() =>
            {
                OnRotateClick?.Invoke(iD);
            });
        }
    }
}
