using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtualConnect
{
    public class ButtonData : MonoBehaviour
    {
        public static Action<uint,bool> clicked;
        public uint id;
        public bool value = true;

        public void StartClicked()
        {
            clicked?.Invoke(id,value);
            value = !value;
        }
    }
}
