using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingProbe : MonoBehaviour
{
    Animator animator;

    public string colliderTag;
    HatRotater hatRotater;

    // Start is called before the first frame update
    void Start()
    {
        animator = transform.parent.GetComponent<Animator>();
        hatRotater = transform.parent.GetComponentInChildren<HatRotater>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == colliderTag)
        {
            Debug.Log("Get ready for chompS!");
            animator.SetTrigger("Bite");
            if (hatRotater != null) hatRotater.SetPosition(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == colliderTag)
        {
            animator.SetTrigger("Close");
            if (hatRotater != null) hatRotater.SetPosition(false);
        }
    }
}
