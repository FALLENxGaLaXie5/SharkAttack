using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableFish : MonoBehaviour, ISpawnable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Spawn(int sideOfScreen)
    {
        transform.Find("AI").GetComponent<FishBrain>().SetDirection(sideOfScreen == 0 ? new Vector2(1, 0) : new Vector2(-1, 0));
    }
}
