using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableSeagull : MonoBehaviour, ISpawnable
{
    public void Spawn(int sideOfScreen)
    {
        transform.Find("AI").GetComponent<SeagullBrain>().SetDirection(sideOfScreen == 0 ? new Vector2(1, 0) : new Vector2(-1, 0));
    }
}
