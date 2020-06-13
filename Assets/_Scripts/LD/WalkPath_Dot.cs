using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkPath_Dot : MonoBehaviour
{
    //Info : 
    public float scale = 1f;
    public int orderInLayer = 0;
    //Maybe :
    public float sizeOfEffect = 2f;

    [System.Serializable]
    public struct nextDot
    {
        public WalkPath_Dot dot;
        
    }

    public List<nextDot> dotsLink = new List<nextDot>();

    public Color colorHere = Color.white;
    


    //TO DO : pause hero here. Unpause hero there
    [MyBox.ButtonMethod()]
    public void TransportHeroHere()
    {
        Perso perso = FindObjectOfType<Perso>();
        perso.SavePosAndTP();

        perso.transform.position = this.transform.position;
        perso.transform.localScale = Vector3.one * scale;

    }



}
