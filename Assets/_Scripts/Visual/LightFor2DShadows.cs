using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LightFor2DShadows : MonoBehaviour
{
    public Vector3 lightDirection = new Vector3(1,1,1);
    private Vector3 pastLightDirection = new Vector3(1, 1, 1);

    private List<ShadowAngle2D> allShadows = new List<ShadowAngle2D>();

    [Range(0, 1)]
    public float ambientLight;
    private float pastAmbientLight;

    public void Awake()
    {
        TakeAllShadowsAngle2D();
    }

    public void Update()
    {
        Vector3 directionUp = this.transform.up;
        lightDirection = this.transform.TransformDirection(directionUp);
        lightDirection.Normalize();
        Debug.DrawRay(this.transform.position, lightDirection, Color.yellow);

        if (lightDirection != pastLightDirection || ambientLight != pastAmbientLight)
        {
            foreach (ShadowAngle2D shadow in allShadows)
            {
                shadow.lightDirectionVector = lightDirection;
                shadow.ambientLight = ambientLight;
            }

            pastLightDirection = lightDirection;
            pastAmbientLight = ambientLight;
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
