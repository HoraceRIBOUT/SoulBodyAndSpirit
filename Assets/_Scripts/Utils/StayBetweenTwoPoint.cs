using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayBetweenTwoPoint : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    public bool scaleOnX = false;
    public bool forgetZDistance = true;

    private Vector3 lastPointA;
    private Vector3 lastPointB;

    private float distanceForOne;
    private Vector3 startScale;
    private float zDepth;

    //for Scale on X 

    void Start()
    {
        if (pointA == null || pointB == null)
            return;
        //the starting point considere this distance for "one" and this distance for "one" 
        //BEWARE : near 0, high risk of going nuts
        lastPointA = pointA.position;
        lastPointB = pointB.position;
        distanceForOne = (lastPointA - lastPointB).magnitude;
        startScale = this.transform.localScale;
        zDepth = this.transform.localPosition.z;

    }

    // Update is called once per frame
    void Update()
    {
        if (pointA == null || pointB == null)
            return;

        if(lastPointA != pointA.position || lastPointB != pointB.position)
        {
            lastPointA = pointA.position;
            lastPointB = pointB.position;
            if (forgetZDistance)
                this.transform.position = (lastPointA + lastPointB) / 2f;
            else
            {
                lastPointA.z = zDepth;
                lastPointB.z = zDepth;
                this.transform.position = (lastPointA + lastPointB) / 2f;
            }

            if (scaleOnX)
                ScaleUpdate();
        }
    }

    void ScaleUpdate()
    {
        float distance = (lastPointA - lastPointB).magnitude;
        float ratio = distance / distanceForOne;
        this.transform.localScale = new Vector3(ratio * startScale.x, startScale.y, startScale.z);
    }
}
