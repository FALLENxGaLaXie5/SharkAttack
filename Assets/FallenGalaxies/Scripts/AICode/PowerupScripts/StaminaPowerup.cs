using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaPowerup : MonoBehaviour, IPowerup
{
    string type = "Stamina";
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
