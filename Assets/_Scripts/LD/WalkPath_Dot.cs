using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkPath_Dot : MonoBehaviour
{
    //Info : 
    public float scale = 1f;
    public int orderInLayer = 0;

    [System.Serializable]
    public struct nextDot
    {
        public WalkPath_Dot dot;
        
    }

    public List<nextDot> dotsLink = new List<nextDot>();

    public Color colorHere = Color.white;
}
