using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Body : MonoBehaviour
{
    List<UI_Limb> limbEquipped = new List<UI_Limb>();

    //emplacement available

    // Start is called before the first frame update
    public void Deploy()
    {
        if(limbEquipped.Count == 0)
        {
            //Init and spawn every needed part
        }
    }

    // Update is called once per frame
    public void Hide()
    {
        //save it. Save the pos.
    }
}
