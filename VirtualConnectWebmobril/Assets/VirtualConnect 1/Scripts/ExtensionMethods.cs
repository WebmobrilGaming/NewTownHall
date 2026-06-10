using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtualConnect
{
    public static class ExtensionMethods
    {
        public static void RotateTransformation(this Transform trans, float value)
        {
            trans.position = Vector3.zero;
            trans.localRotation = Quaternion.identity;
            trans.localScale = new Vector3(1, 1, 1);
        }
    }
}
