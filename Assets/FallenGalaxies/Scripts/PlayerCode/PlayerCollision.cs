using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    PlayerControl playerControl;


    [SerializeField] int damage = 1;
    // Start is called before the first frame update
    void Start()
    {
        playerControl = transform.parent.GetComponent<PlayerControl>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {    
        if (collision.gameObject.tag == "Wall")
        {
            transform.rotation = Quaternion.identity;
            this.transform.parent.localScale = new Vector3(-transform.parent.localScale.x, transform.parent.localScale.y, transform.parent.localScale.z);
            playerControl.SetFacingLeft(!playerControl.GetFacingLeft());
        }

    }
}
