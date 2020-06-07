using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowAngle2D : MonoBehaviour
{
    public List<SpriteRenderer> frontShadows = new List<SpriteRenderer>();
    public List<SpriteRenderer> upShadows = new List<SpriteRenderer>();
    public List<SpriteRenderer> rightShadows = new List<SpriteRenderer>();
    public List<SpriteRenderer> leftShadows = new List<SpriteRenderer>();

    public Vector3 lightDirectionVector = new Vector3(1, 1, 1);
    private Vector3 pastLightDirectionVector = new Vector3(1, 1, 1);


    // Update is called once per frame
    void Update()
    {
        if (lightDirectionVector != pastLightDirectionVector)
        {
            ChangeColorOfShadow();

            pastLightDirectionVector = lightDirectionVector;
        }
    }

    void ChangeColorOfShadow()
    {
        foreach (SpriteRenderer sP in frontShadows)
        {
            Color frontColor = Color.white;
            Vector3 myDirection = Vector3.forward;
            frontColor.a = Vector3.Dot(lightDirectionVector, myDirection);
            sP.color = frontColor;
        }
        foreach (SpriteRenderer sP in upShadows)
        {
            Color upColor = Color.white;
            Vector3 myDirection = Vector3.up;
            upColor.a = Vector3.Dot(lightDirectionVector, myDirection);
            sP.color = upColor;
        }

        foreach (SpriteRenderer sP in leftShadows)
        {
            Color leftColor = Color.white;
            Vector3 myDirection = Vector3.left * sP.transform.rotation.eulerAngles.z * (sP.flipX?-1:1);
            leftColor.a = Vector3.Dot(lightDirectionVector, myDirection);
            sP.color = leftColor;
        }

        foreach (SpriteRenderer sP in rightShadows)
        {
            Color rightColor = Color.white;
            Vector3 myDirection = Vector3.right * sP.transform.rotation.eulerAngles.z * (sP.flipX ? -1 : 1);
            rightColor.a = Vector3.Dot(lightDirectionVector, myDirection);
            sP.color = rightColor;
        }

    }
}
