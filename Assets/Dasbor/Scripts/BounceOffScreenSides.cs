using Assets.Dasbor.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceOffScreenSides : MonoBehaviour
{
    public int numberOfBounces = 3;

    bool enteredScreen = false;
    IBrain brain;
    bool bouncing = false;

    private void Start()
    {
        brain = transform.parent.GetComponentInChildren<IBrain>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (bouncing && numberOfBounces > 0 && collision.gameObject.tag == "Wall")
        {
            //Debug.Log("Bounce");
            brain.SetDirection(new Vector2(brain.GetDirection().x * -1, 0));
            numberOfBounces--;
            if(numberOfBounces <= 0)
            {
                bouncing = false;
            }
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            transform.parent.Find("BounceCollider").gameObject.SetActive(true);
            bouncing = true;
        }
    }
}
