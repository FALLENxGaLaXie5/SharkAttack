using Assets.Dasbor.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceOffScreenSidesModified : MonoBehaviour
{
    public int numberOfBounces = 3;

    bool enteredScreen = false;
    IBrain brain;
    bool bouncing = true;

    BoatLightLerper boatLightLerper;

    private void Start()
    {
        brain = transform.parent.GetComponentInChildren<IBrain>();
        boatLightLerper = transform.parent.GetComponentInChildren<BoatLightLerper>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print("Bounce me!");
        if (bouncing && numberOfBounces > 0 && collision.gameObject.tag == "Wall")
        {
            Debug.Log("Bounce");
            brain.SetDirection(new Vector2(brain.GetDirection().x * -1, 0));
            numberOfBounces--;
            if(numberOfBounces <= 0)
            {
                bouncing = false;
            }
            boatLightLerper.SetFacingLeft(!boatLightLerper.GetFacingLeft());
        }
    }
}
