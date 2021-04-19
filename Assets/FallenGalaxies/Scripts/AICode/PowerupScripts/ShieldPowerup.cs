using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerup : MonoBehaviour, IPowerup
{
    string type = "Shield";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivatePowerup()
    {

    }

    public string GetType()
    {
        return this.type;
    } 
}
