using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPredator : MonoBehaviour
{
    public int damage = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Debug.Log("Eat");
            collision.gameObject.GetComponent<IEatable>().Eaten(damage);
        }
    }
}
