using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpeed : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rigidBody;
    int speedHash = Animator.StringToHash("Speed");
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();

        
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat(speedHash, rigidBody.velocity.magnitude / 2f);
    }
}
