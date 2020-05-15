using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    public SpriteOpacity bgOfThisRoom;


    public ImageOpacity imageFakeSlider;

    public Animator UIfightAnim;


    public void LaunchFight()
    {
        if (!UIfightAnim.gameObject.activeSelf)
            UIfightAnim.gameObject.SetActive(true);
        StartCoroutine(doubleOpacity());
    }

    IEnumerator doubleOpacity()
    {
        float lerpVal = bgOfThisRoom.opactiy;
        while(lerpVal < 1)
        {
            bgOfThisRoom.opactiy = lerpVal;
            imageFakeSlider.opactiy = lerpVal;
            lerpVal += Time.deltaTime;
            yield return new WaitForSeconds(1f/60f);
        }
        //call some animator ?
        UIfightAnim.SetTrigger("start");
    }

    public void EndFight()
    {
        StartCoroutine(doubleOpacityDown());
        UIfightAnim.SetTrigger("end");
    }

    IEnumerator doubleOpacityDown()
    {
        float lerpVal = bgOfThisRoom.opactiy;
        while (lerpVal > 0)
        {
            bgOfThisRoom.opactiy = lerpVal;
            imageFakeSlider.opactiy = lerpVal;
            lerpVal -= Time.deltaTime;
            yield return new WaitForSeconds(1f / 60f);
        }
        //call some animator ?
    }

}
