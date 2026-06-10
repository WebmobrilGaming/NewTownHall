using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPC : MonoBehaviour
{
    public  enum Type
    {
        SITTING_IN_MEETING,
        SITTING_IN_PLACE,
        SITTING_TALKING,
        STANDING_IDLE,
        STANDING_TALKING,
        STANDING_WITH_BRIEFCASE
    }

    public Type NPC_Type;
    public abstract void SetAnim();
}
