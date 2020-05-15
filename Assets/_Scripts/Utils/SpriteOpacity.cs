using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SpriteOpacity : MonoBehaviour
{
    public List<SpriteRenderer> spriteToAffect;
    
    [Range(0,1)]
    public float opactiy;

    //public bool reverseY = false;

    public void Update()
    {
        renderOpacity();
    }

    public void renderOpacity()
    {
        Color colInMemory = Color.white;
        if (spriteToAffect.Count == 0)
            return;

        foreach (SpriteRenderer sR in spriteToAffect)
        {
            colInMemory = sR.color;
            colInMemory.a = opactiy;
            sR.color = colInMemory;
        }
    }


    [MyBox.ButtonMethod]
    public void GetAllSpriteInHierarchy()
    {
        spriteToAffect.Clear();
        foreach (SpriteRenderer sR in GetComponentsInChildren<SpriteRenderer>())
        {
            spriteToAffect.Add(sR);
        }
    }

}
