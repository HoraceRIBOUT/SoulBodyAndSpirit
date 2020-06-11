using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ShadowAngle2D : MonoBehaviour
{
    public List<SpriteRenderer> frontShadows = new List<SpriteRenderer>();
    public List<SpriteRenderer> upShadows = new List<SpriteRenderer>();
    public List<SpriteRenderer> rightShadows = new List<SpriteRenderer>();
    public List<SpriteRenderer> leftShadows = new List<SpriteRenderer>();

    [HideInInspector]
    public float ambientLight;
    private float pastAmbientLight;

    public Vector3 lightDirectionVector = new Vector3(1, 1, 1);
    private Vector3 pastLightDirectionVector = new Vector3(1, 1, 1);

    private Vector3 pastRotation = Vector3.zero;


    private void Start()
    {
        pastRotation = this.transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (lightDirectionVector != pastLightDirectionVector || pastRotation != this.transform.rotation.eulerAngles || ambientLight != pastAmbientLight)
        {
            ChangeColorOfShadow();

            pastRotation = this.transform.rotation.eulerAngles;
            pastLightDirectionVector = lightDirectionVector;
            pastAmbientLight = ambientLight;
        }
    }

    void ChangeColorOfShadow()
    {
        TreatEachShadows(frontShadows, Vector3.back);
        TreatEachShadows(upShadows, Vector3.up);//influence from rotation

        TreatEachShadows(leftShadows, Vector3.left);
        TreatEachShadows(rightShadows, Vector3.right);

        // new Vector3(0, sP.transform.rotation.eulerAngles.z / 90, 1 - sP.transform.rotation.eulerAngles.z / 90); // ==> 0 basic left -90 ==> up

    }

    private void TreatEachShadows(List<SpriteRenderer> listShadows, Vector3 vec)
    {
        foreach (SpriteRenderer sP in listShadows)
        {
            Color color = Color.black;
            Vector3 myDirection = vec;
            float dot = Mathf.Clamp01(Vector3.Dot(lightDirectionVector, myDirection));
            color.a = 1 - (dot + ambientLight);
            sP.color = color;
        }
    }
}
