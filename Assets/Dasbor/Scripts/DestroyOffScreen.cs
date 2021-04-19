using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOffScreen : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "FishDisposal")
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
