using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parralaxe : MonoBehaviour
{
    public bool overrideZProfounder = false;
    public float depth = 0;

    private Transform cameraTransform;

    private Vector3 firstCameraPosition;
    private Vector3 firstObjectPosition;
    private Vector3 lastCameraPosition;

    private void Start()
    {
        cameraTransform = GameManager.Instance.cam.transform;

        firstCameraPosition = cameraTransform.position;
        lastCameraPosition = cameraTransform.position;
        firstObjectPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (lastCameraPosition != cameraTransform.position)
        {
            this.transform.position = firstObjectPosition + (firstCameraPosition - lastCameraPosition) * depth;
            lastCameraPosition = cameraTransform.position;
        }
    }
}
