using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirMovement : MonoBehaviour
{
    [Header("Gravity Scales")]
    [Tooltip("Air Gravity Scale")] [SerializeField] float airGravity = 0.9f;
    [Tooltip("Water Gravity Scale")] [SerializeField] float waterGravity = 0.02f;


    bool inAir = false;
    Rigidbody2D rigidBody;
    PlayerControl playerControls;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = transform.parent.GetComponent<Rigidbody2D>();
        playerControls = transform.parent.GetComponent<PlayerControl>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Air")
        {
            rigidBody.gravityScale = airGravity;
            inAir = true;
            playerControls.SetTouchControlsEnabled(false);
        }        
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Air")
        {
            rigidBody.gravityScale = waterGravity;
            inAir = false;
            playerControls.SetTouchControlsEnabled(true);
            playerControls.ResetAutomaticAirTurningRate();
        }    
    }

    public bool GetInAir()  { return inAir; }
}
