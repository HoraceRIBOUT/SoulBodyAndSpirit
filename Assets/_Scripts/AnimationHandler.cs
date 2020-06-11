using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{

    public List<Animator> animators = new List<Animator>();
    public static List<string> animatorsName = new List<string>();
   
    public void Awake()
    {
        if(animatorsName == null || animatorsName.Count == 0)
        {
            UpdateAnimatorsNameList();
        }
    }

    [ContextMenu("Update Animators List")]
    public void UpdateAnimatorsNameList()
    {
        //TO DO : Update only when needed
        if (animators.Count == 0)
        {
            foreach (Animator ani in GameObject.FindObjectsOfType<Animator>())
                animators.Add(ani);
        }

        
        animatorsName = new List<string>();
        foreach (Animator an in animators)
        {
            //Debug.Log("One more : " + an.name);
            animatorsName.Add(an.name);
        }
        //Debug.Log("animatorsName have now " + animatorsName.Count + "member in it");
    }

    public float PlayAnimation(int indexOfAnimator, string nameOfTrigger, bool waitForEndOfAnimationOrNot)
    {
        Debug.Log("index animator :" + indexOfAnimator);
        animators[indexOfAnimator].SetTrigger(nameOfTrigger);
        return waitForEndOfAnimationOrNot ? 3 : 0;//same as next one
    }

    public float PlayAnimation(int indexOfAnimator, string nameOfBool, bool valueOfBool, bool waitForEndOfAnimationOrNot)
    {
        animators[indexOfAnimator].SetBool(nameOfBool,valueOfBool);
        return waitForEndOfAnimationOrNot ? 3 : 0;//need to wait not 3 s but the lenght of the next animation OR to wait until it have return to the idle first.... well, that's complicated
    }


}
