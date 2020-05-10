using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Interaction {

    public string id;
    public string nameInChoice;
    public bool item;
    //Get rid of this one
    public string pathToFollow;

    public bool active = true;

    public bool changeCSV = false;
}
