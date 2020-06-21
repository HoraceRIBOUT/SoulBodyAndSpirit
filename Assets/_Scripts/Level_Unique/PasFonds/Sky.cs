using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Sky : MonoBehaviour
{
    //This is a stupid test :
    public LightFor2DShadows lightDir;
    private Vector3 lastDir;
    private float transparencyForCloud;
    private List<float> transparencyForBG = new List<float>();

    public List<SpriteRenderer> cloudAffect = new List<SpriteRenderer>();
    public List<SpriteRenderer> backgroundSprite = new List<SpriteRenderer>();

    public Gradient fromDayToDawnToDark = new Gradient();

    [MyBox.ButtonMethod()]
    public void Start()
    {
        Debug.Log("Ok ?");
        transparencyForCloud = cloudAffect[0].color.a;
        transparencyForBG.Clear();
        foreach (SpriteRenderer sR in backgroundSprite)
            transparencyForBG.Add(sR.color.a);
    }

    public bool launch = false;
    public void Update()
    {
        if (!launch)
            return;

        if (lightDir.lightDirection != lastDir)
        {
            float val = Vector3.Dot(lightDir.lightDirection, this.transform.up);
            Color c = fromDayToDawnToDark.Evaluate((val + 1f) / 2f);

            c.a = transparencyForCloud;
            foreach (SpriteRenderer sR in cloudAffect)
                sR.color = c;
            int i = 0;
            foreach (SpriteRenderer sR in backgroundSprite)
            {
                c.a = transparencyForBG[i];
                sR.color = c;
                i++;
            }

            lastDir = lightDir.lightDirection;
        }
    }

}
