using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CinematicBar : MonoBehaviour
{
    [Range(0.1f, 3f)]
    public float speedOfBars = 1f;

    public float goalPos = 0.2f;
    public UnityEngine.RectTransform upBar;
    public UnityEngine.RectTransform downBar;

    float lerpValue = 0;//0 mean hidden, 1 mean display

    private void Start()
    {
        RepositionneBothCiBar(lerpValue);
    }

    public void DisplayCibar(bool display, float speed)
    {
        speedOfBars = speed;
        if (display)
            DisplayCibar();
        else
            HideCibar();
    }

    [MyBox.ButtonMethod()]
    void DisplayCibar()
    {
        Debug.Log("Display ! ");
        StopAllCoroutines();
        StartCoroutine(displayCurrentCibar());
    }

    IEnumerator displayCurrentCibar()
    {
        while (lerpValue <= 1)
        {
            lerpValue += Time.deltaTime * speedOfBars;
            lerpValue = lerpValue > 1 ? 1 : lerpValue;
            RepositionneBothCiBar(lerpValue);
            yield return new WaitForSeconds(0.01f);
        }
        RepositionneBothCiBar(1);
    }

    [MyBox.ButtonMethod()]
    void HideCibar()
    {
        Debug.Log("Hide ! ! ");
        StopAllCoroutines();
       StartCoroutine(hiddeCurrentCibar());
    }

    IEnumerator hiddeCurrentCibar()
    {
        while (lerpValue >= 0)
        {
            lerpValue -= Time.deltaTime * speedOfBars;
            lerpValue = lerpValue < 0 ? 0 : lerpValue;
            RepositionneBothCiBar(lerpValue);
            yield return new WaitForSeconds(0.01f);
        }
        RepositionneBothCiBar(0);
    }

    void RepositionneBothCiBar(float value)
    {
        upBar.anchorMin = new Vector2(0, 1 - (goalPos * value));
        upBar.anchorMax = new Vector2(1, 1 + (goalPos * (1 - value)));
        downBar.anchorMin = new Vector2(0, - (goalPos * (1 - value)));
        downBar.anchorMax = new Vector2(1,   (goalPos * value));
    }

}
