using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class MoveWithoutChild : MonoBehaviour
{
    public bool activate = true;
    private Vector3 lastPosition;

    private void Awake()
    {
        lastPosition = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (lastPosition != this.transform.localPosition)
        {
            if (activate)
                ChildReplacement();
            lastPosition = this.transform.localPosition;
        }
    }

    void ChildReplacement()
    {
        Vector3 offset = lastPosition - this.transform.localPosition;
        
        offset = Quaternion.Inverse(this.transform.rotation) * offset;

        offset.x /= this.transform.localScale.x;
        offset.y /= this.transform.localScale.y;
        offset.z /= this.transform.localScale.z;

        for (int childI = 0; childI < transform.childCount; childI++)
            transform.GetChild(childI).localPosition += offset;
    }
}
