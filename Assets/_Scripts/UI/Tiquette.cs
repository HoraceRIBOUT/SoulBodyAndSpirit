using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tiquette : MonoBehaviour {
    
    public RectTransform rectT;
    public Text textT;
    public Button buttT;

    public Interaction inter;

    public void init(float pixelSize, int i, int fontSize, UnityEngine.Events.UnityAction action, Interaction interact, Vector2 scaleRot)
    {
        rectT.sizeDelta = new Vector2(rectT.sizeDelta.x, pixelSize);
        rectT.localPosition = new Vector2(0, -pixelSize * i);
        rectT.anchoredPosition = new Vector2(0, rectT.anchoredPosition.y);
        rectT.pivot = new Vector2(rectT.pivot.x, scaleRot.y == 1 ? 1 : 0);
        rectT.localScale = scaleRot;
        textT.text = interact.nameInChoice;
        textT.fontSize = fontSize;
        buttT.onClick.RemoveAllListeners();
        buttT.onClick.AddListener(action);
        inter = interact;
    }
    

}
