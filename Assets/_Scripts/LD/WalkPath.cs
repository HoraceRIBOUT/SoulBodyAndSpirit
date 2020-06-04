using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class WalkPath : MonoBehaviour
{
    public List<WalkPath_Dot> wholePath_Dots;

    public bool showTheWholePath = false;

    public struct closestPointResult
    {
        public WalkPath_Dot firstPoint;
        public WalkPath_Dot lastPoint;
        public float ratio;
    }

    public closestPointResult ClosestDot(Vector3 positionInSpace)
    {
        //IDEA ! Get the TWO closest point ! Then, verify if they are link together !
        //          If not : return one error but goes to the closest point anyway
        //If yes : return one point, other point, ratio of proximity
        closestPointResult res;
        res.firstPoint = null;
        res.lastPoint = null;
        res.ratio = 0f;


        //distance min
        float minDist = 100 * 100;
        res.firstPoint = wholePath_Dots[0];
        foreach (WalkPath_Dot dot in wholePath_Dots)
        {
            //min dist
            float dist = (positionInSpace - dot.transform.position).sqrMagnitude;
            if (minDist > dist)
            {
                minDist = dist;
                res.firstPoint = dot;
            }
        }
        return res;
    }

    public void GoesToClosestDot(Vector3 positionInSpace)
    {
        //for now empty
    }

    private void Start()
    {
        //EMPTY : EXECUTE ALWAYS
    }

    private void OnGUI()
    {
        if (showTheWholePath)
        {
            foreach (WalkPath_Dot dot in wholePath_Dots)
            {
                foreach (WalkPath_Dot.nextDot nextDott in dot.dotsLink)
                {
                    //Verify if not already exist : for now, double draw, don't care
                    Debug.DrawLine(dot.transform.position, nextDott.dot.transform.position, Color.red);
                }
            }
        }
    }


    //DUmb copy pasta//Please reread it
    private List<WalkPath_Dot> dotDisponible = new List<WalkPath_Dot>();
    private List<WalkPath_Dot> dotVisite = new List<WalkPath_Dot>();



    public List<WalkPath_Dot> StartPath(closestPointResult fromThisDot, closestPointResult toThatDot)
    {
        //TO DO : treat  that
        return StartPath(fromThisDot.firstPoint, toThatDot.firstPoint);
    }

    public List<WalkPath_Dot> StartPath(WalkPath_Dot fromThisDot, closestPointResult toThatDot)
    {
        //TO DO : treat  that
        return StartPath(fromThisDot, toThatDot.firstPoint);
    }

    public List<WalkPath_Dot> StartPath(closestPointResult fromThisDot, WalkPath_Dot toThatDot)
    {
        //TO DO : treat  that
        return StartPath(fromThisDot.firstPoint, toThatDot);
    }

    public List<WalkPath_Dot> StartPath(WalkPath_Dot fromThisDot, WalkPath_Dot toThatDot)
    {
        dotDisponible.Clear();
        dotVisite.Clear();

        bool allInDisponible = (dotDisponible.Contains(fromThisDot) && dotDisponible.Contains(toThatDot));
        return GetPath(fromThisDot, toThatDot, allInDisponible);
    }


    public List<WalkPath_Dot> GetPath(WalkPath_Dot fromThisDot, WalkPath_Dot toThatDot, bool onlyDisponibleDot)
    {
        List<WalkPath_Dot> res = new List<WalkPath_Dot>();
        res.Clear();

        if (fromThisDot == toThatDot)
        {
            res.Add(fromThisDot);
        }
        else
        {
            dotVisite.Add(fromThisDot);
            foreach (WalkPath_Dot.nextDot nextPoint in fromThisDot.dotsLink)
            {
                if (!dotVisite.Contains(nextPoint.dot) && (dotDisponible.Contains(nextPoint.dot) || !onlyDisponibleDot) && !res.Contains(toThatDot))
                {
                    res.InsertRange(0, GetPath(nextPoint.dot, toThatDot, onlyDisponibleDot));
                }
            }
            if (res.Contains(toThatDot))
            {
                res.Insert(0, fromThisDot);
            }

        }

        return res;
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
