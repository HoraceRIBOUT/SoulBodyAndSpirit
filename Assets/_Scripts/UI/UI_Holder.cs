using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Holder : MonoBehaviour {
    
    public DescAndDial desc;

    [Header("Pop Up")]
    //Popup part
    public PopUp currentPopUp;
    public GameObject popUpPrefab;
    public float pixelSize;
    public float fontSize;
    public Vector2 halfSize;
    private List<Interaction> interactionActive = new List<Interaction>();


    [Header("Cinematic bar")]
    //Cinematic bar
    public CinematicBar cinematicBar;

	// Use this for initialization
	void Start () {
        pixelSize = Screen.height / 20;
        fontSize = pixelSize * 0.75f;   //for spacing 
        halfSize = Vector2.up * Screen.height / 2 + Vector2.right * Screen.width / 2;
    }
	

    public void Finish()
    {
        print("Yep, ça passe chez moi");
        desc.Finish();

    }

    public void Description(Step givenStep){
        desc.Description(givenStep);
    }

    public void Dialogue(Step givenStep)
    {
        desc.Dialogue(givenStep);
    }
    
    public void OpenPopUp(Vector2 position, List<Interaction> interactions)
    {
        if (currentPopUp == null)
            currentPopUp = Instantiate(popUpPrefab,this.transform).GetComponent<PopUp>();

        interactionActive.Clear();
        foreach (Interaction inter in interactions)
        {
            if (inter.active)
                if(!inter.item || GameManager.Instance.scenario.inventaire.Contains(Utils.itemStringToEnum(inter.id)))
                    interactionActive.Add(inter);
        }

        currentPopUp.init(position, interactionActive, pixelSize, fontSize, halfSize);
    }
    
}
