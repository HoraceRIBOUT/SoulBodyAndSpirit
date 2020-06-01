using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkPath : MonoBehaviour
{
    public List<WalkPath_Dot> wholePath_Dots;

    public void GoesToClosestDot(Vector3 positionInSpace)
    {
        //distance min
        foreach (WalkPath_Dot dot in wholePath_Dots)
        {
            //min dist
        }




    }

    public void GoesToThisPoint()
    {

    }












    [MyBox.ButtonMethod()]
    public void FillPathWithChildDot()
    {
        wholePath_Dots.Clear();
        foreach (WalkPath_Dot dot in GetComponentsInChildren<WalkPath_Dot>())
        {
            wholePath_Dots.Add(dot);
        }
    }
}
