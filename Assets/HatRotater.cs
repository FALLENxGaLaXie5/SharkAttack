using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatRotater : MonoBehaviour
{
    Vector3 originalPosition;
    Vector3 rotatedPosition = new Vector3(-0.75f, 0.89f, 0);

    Vector3 originalRotation = Vector3.zero;
    Vector3 rotatedRotation = new Vector3(0, 0, -34);
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.localPosition;
        //Debug.Log("originalPosition: " + originalPosition);
    }

    public void SetPosition(bool rotated)
    {
        if (rotated)
        {
            transform.localPosition = rotatedPosition;
            transform.localRotation = Quaternion.Euler(rotatedRotation.x, rotatedRotation.y, rotatedRotation.z);
        }
        else
        {
            transform.localPosition = originalPosition;
            transform.localRotation = Quaternion.Euler(originalRotation.x, originalRotation.y, originalRotation.z);
        }
    }
}
