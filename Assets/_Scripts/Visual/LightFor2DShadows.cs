using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFor2DShadows : MonoBehaviour
{
    public Vector3 lightDirection = new Vector3(1,1,1);
    public Vector3 pastLightDirection = new Vector3(1, 1, 1);

    private List<ShadowAngle2D> allShadows = new List<ShadowAngle2D>();

    public void Awake()
    {
        TakeAllShadowsAngle2D();
    }

    public void Update()
    {
        if (lightDirection != pastLightDirection)
        {
            foreach (ShadowAngle2D shadow in allShadows)
            {
                shadow.lightDirectionVector = lightDirection;
            }

            pastLightDirection = lightDirection;
        }
        
    }
    
    [MyBox.ButtonMethod()]
    public void TakeAllShadowsAngle2D()
    {
        allShadows.Clear();
        foreach (ShadowAngle2D shadow in FindObjectsOfType<ShadowAngle2D>())
        {
            allShadows.Add(shadow);
        }
    }
}
