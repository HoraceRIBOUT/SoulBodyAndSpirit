using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionSurcouche : MonoBehaviour {

    public RectTransform rectTransform;
    public UnityEngine.UI.Text desc;

	// Use this for initialization
	void Start () {
        desc.fontSize = (int)(((float)rectTransform.rect.height / 4));
        //desc.fontSize *= 2;
    }
	
}
