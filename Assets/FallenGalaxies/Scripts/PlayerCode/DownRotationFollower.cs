using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownRotationFollower : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector2(playerTransform.position.x, this.transform.position.y);
    }
}
