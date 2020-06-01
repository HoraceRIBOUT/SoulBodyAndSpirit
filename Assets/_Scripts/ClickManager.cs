using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{

    //maybe get all call of click redirect here ? and is the one who treat/recall them ? (by calling Call(this) in the other way ? 
    //reaching all "mouse Up" and "onClick" (just looking at the register)

        //I like it. Cool idea

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            GameManager.Instance.scenario.currentRoom.walkPath.GoesToClosestDot(Input.mousePosition);
        }
    }


}
