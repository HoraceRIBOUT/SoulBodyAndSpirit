using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkPath_Dot : MonoBehaviour
{
    //Info : 
    public float scale = 1f;
    public int orderInLayer = 0;

    struct nextDot
    {
        WalkPath_Dot dot;
        
    }

    public Color colorHere = Color.white;
}
