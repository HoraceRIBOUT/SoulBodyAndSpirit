using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAnimRotPosScale : MonoBehaviour
{
    [Range(0,1)]
    public float startOffset = 0.3f;
    public bool goesEnd = true;
    public float speed = 1f;

    [Header("Position")]
    public bool movePos = true;
    public Vector3 startPosition = Vector3.zero;
    public Vector3 endPosition = Vector3.zero;
    public AnimationCurve wayToGoesPos;
    
    [Header("Rotation")]
    public bool moveRot = true;
    public Vector3 startRotation = Vector3.zero;
    public Vector3 endRotation = Vector3.zero;
    public AnimationCurve wayToGoesRot;

    [Header("Scale")]
    public bool moveScale = true;
    public Vector3 startScale = Vector3.one;
    public Vector3 endScale = Vector3.one;
    public AnimationCurve wayToGoesScale;

    private float lerpValue = 0;

    private void Start()
    {
        lerpValue = startOffset;
    }

    // Update is called once per frame
    void Update()
    {
        lerpValue += Time.deltaTime * (goesEnd ? 1 : -1) * (speed);

        if(lerpValue < 0 )
        {
            lerpValue = 0;
            goesEnd = true;
        }
        if (lerpValue > 1)
        {
            lerpValue = 1;
            goesEnd = false;
        }
        if(movePos)
            this.transform.localPosition =Vector3.Lerp(startPosition, endPosition, wayToGoesPos.Evaluate(lerpValue));
        if(moveRot)
            this.transform.localRotation = Quaternion.Euler(Vector3.Lerp(startRotation, endRotation, wayToGoesRot.Evaluate(lerpValue)));
        if(moveScale)
            this.transform.localRotation = Quaternion.Euler(Vector3.Lerp(startRotation, endRotation, wayToGoesScale.Evaluate(lerpValue)));

    }




    [MyBox.ButtonMethod()]
    public void SetThreeOfThem()
    {
        SetPositionToMyPosition();
        SetRotationToMyRotation();
        SetScaleToMyScale();
    }

    [MyBox.ButtonMethod()]
    public void SetPositionToMyPosition()
    {
        startPosition = transform.localPosition;
        endPosition = transform.localPosition;
    }

    [MyBox.ButtonMethod()]
    public void SetRotationToMyRotation()
    {
        startRotation = transform.localRotation.eulerAngles;
        endRotation = transform.localRotation.eulerAngles;
    }

    [MyBox.ButtonMethod()]
    public void SetScaleToMyScale()
    {
        startScale = transform.localScale;
        endScale = transform.localScale;
    }

}
