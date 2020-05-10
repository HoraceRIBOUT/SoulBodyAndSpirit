using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour {
    
    public List<Tiquette> etiquettes = new List<Tiquette>();

    public Transform parentTiquette; 
    public GameObject etiquette;
    RectTransform rectParent;
    RectTransform rectThis;
    
    // Use this for initialization
    void Start () {
    }

    public void init(Vector2 position, List<Interaction> interactions, float pixelSize, float fontSize, Vector2 halfSize)
    {
        //Clean
        CleanEtiquettes();

        GameObject gO;
        Tiquette tiquette;
        int size = interactions.Count;
        Vector2 scaleRot = Vector2.one;
        if(position.x > halfSize.x)
            scaleRot += Vector2.left * 2;
        if (position.y < halfSize.y)
            scaleRot += Vector2.down * 2;

        if (rectThis == null || rectParent == null)
        {
            rectParent = parentTiquette.GetComponent<RectTransform>();
            rectThis = this.GetComponent<RectTransform>();
        }

        //Own size
        rectParent.sizeDelta = new Vector2(rectParent.sizeDelta.x, pixelSize * size);
        rectParent.localScale = scaleRot;
        rectThis.position = position;

        /*Creation tiquette*/
        if (etiquettes.Count < size)
        {
            for (int i = etiquettes.Count; i < size; i++)
            {
                gO = Instantiate(etiquette, parentTiquette);
                tiquette = gO.GetComponent<Tiquette>();
                etiquettes.Add(tiquette);
            }
        }

        //Set tiquette to value
        for (int i = 0; i < size; i++)
        {
            tiquette = etiquettes[i];
            int value = i;
            tiquette.init(pixelSize, i, (int)fontSize, delegate { ClickOn(value); }, interactions[i], scaleRot);
            tiquette.gameObject.SetActive(true);
        }
    }


    void ClickOn(int indexHere)
    {
        GameManager.Instance.scenario.ExecutePath(etiquettes[indexHere].inter.pathToFollow);
        ClosePopUp();
    }

    public void ClosePopUp()
    {
        CleanEtiquettes();
        rectParent.sizeDelta = new Vector2(rectParent.sizeDelta.x, 0);

        GameManager.Instance.scenario.currentZone = null;
    }

    public void CleanEtiquettes()
    {
        foreach (Tiquette ti in etiquettes)
        {
            ti.gameObject.SetActive(false);
        }
    }





}
