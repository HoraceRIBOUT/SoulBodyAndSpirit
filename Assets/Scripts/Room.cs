using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    public string id;
    public List<Zone> zones = new List<Zone>();

    // Use this for initialization
    void Start () {
#if UNITY_EDITOR
        foreach (Zone z in GetComponentsInChildren<Zone>())
        {
            if (!zones.Contains(z))
            {
                Debug.LogError(z.name + " (id = "+z.id+") n'a pas été ajouté dans la Room :" + this.name);
            }
        }
#endif
    }


    public void setActiveZone(string zoneID, bool b)
    {
        foreach (Zone z in zones)
        {
            if(z.id == zoneID)
            {
                z.gameObject.SetActive(b);
            }
        }
    }

    public void setActiveInteractionInZone(string zoneID, string interID, bool b)
    {
        foreach (Zone z in zones)
        {
            if (z.id == zoneID)
            {
                Debug.Log("Zone find");
                z.setActiveInter(interID, b);
            }
        }
    }

}
