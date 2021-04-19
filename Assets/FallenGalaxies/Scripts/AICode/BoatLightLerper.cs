using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatLightLerper : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] float maxRotation = 15f;
    [SerializeField] float defaultDown = 210f;

    float defaultLeftFaceRotation;
    bool facingLeft = false;
    float inverseAngle = 0f;
    // Start is called before the first frame update
    void Start()
    {
        defaultLeftFaceRotation = (270f - defaultDown) + 90f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!facingLeft)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, (maxRotation * Mathf.Sin(Time.time * speed)) + defaultDown);
        }
        else
        {
            inverseAngle = 270f - transform.localEulerAngles.z;
            transform.rotation = Quaternion.Euler(0f, 0f, (maxRotation * Mathf.Sin(Time.time * speed)) + defaultLeftFaceRotation);
        }
    }

    public void SetFacingLeft(bool newFacingLeft)
    {
        this.facingLeft = newFacingLeft;
    }

    public bool GetFacingLeft()
    {
        return this.facingLeft;
    }
}
