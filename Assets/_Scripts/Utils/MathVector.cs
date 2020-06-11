using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathVector : MonoBehaviour
{
    static Vector3 DirectionToEulerAngles(Vector3 direction)
    {
        float dot = Vector3.Dot(Vector3.forward, direction);
        float sign = Mathf.Sign(Vector3.Cross(Vector3.forward, direction).y);
        
        return Vector3.forward * (dot - 1) * 90 * sign;
    }

    static Quaternion DirectionToQuaternion(Vector3 direction)
    {
        Vector3 eulerAngles = DirectionToEulerAngles(direction);
        return Quaternion.Euler(eulerAngles);
    }

    /// <summary>
    /// Null for now
    /// </summary>
    /// <param name="angles"></param>
    /// <returns></returns>
    static Vector3 EulerAnglesToDirection(Vector3 angles)
    {
        return Vector3.one;
    }

}
