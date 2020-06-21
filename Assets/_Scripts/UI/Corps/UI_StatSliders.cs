using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class UI_StatSliders : MonoBehaviour
{
    public Slider thisSlider;

    public float maxValue = 15;
    public Text handlerVal;

    public void Update()
    {
        if(thisSlider == null)
        {
            thisSlider = GetComponent<Slider>();
            return;
        }
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        handlerVal.text = ((int)(maxValue * thisSlider.value)).ToString();
    }

    public void ChangeValue(float value)
    {
        thisSlider.value = Mathf.Clamp01(value / maxValue);
        UpdateVisual();
    }

}
