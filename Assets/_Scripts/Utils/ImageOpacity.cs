using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class ImageOpacity : MonoBehaviour
{
    public List<Image> imageToAffect;

    [Range(0, 1)]
    public float opactiy;

    public void Update()
    {
        renderOpacity();
    }

    public void renderOpacity()
    {
        Color colInMemory = Color.white;
        if (imageToAffect.Count == 0)
            return;
        foreach (Image im in imageToAffect)
        {
            colInMemory = im.color;
            colInMemory.a = opactiy;
            im.color = colInMemory;
        }
    }

}
