using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rot : MonoBehaviour
{
    public Vector3 rotationPerSecond = new Vector3(0, 0, 360);

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(rotationPerSecond * Time.deltaTime);
    }
}
