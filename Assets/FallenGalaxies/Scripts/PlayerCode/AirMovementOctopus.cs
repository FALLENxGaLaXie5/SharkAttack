using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirMovementOctopus : MonoBehaviour
{
    [Header("Gravity Scales")]
    [Tooltip("Air Gravity Scale")] [SerializeField] float airGravity = 0.9f;
    [Tooltip("Water Gravity Scale")] [SerializeField] float waterGravity = 0.02f;


    bool inAir = false;
    Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = transform.parent.GetComponent<Rigidbody2D>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Air")
        {
            rigidBody.gravityScale = airGravity;
            inAir = true;
        }        
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Air")
        {
            rigidBody.gravityScale = waterGravity;
            inAir = false;
        }    
    }

    public bool GetInAir()  { return inAir; }
}
