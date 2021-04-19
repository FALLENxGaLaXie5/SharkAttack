using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatableFish : MonoBehaviour, IEatable
{
    public int health = 1;
    public int score = 100;

    public GameObject floatingScore;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Eaten(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GameObject scoreObject = Instantiate(floatingScore, transform.parent.position, Quaternion.identity);
            scoreObject.GetComponent<FloatingScore>().SetScore(score);
            Scorer.UpdateScore(score);
            Destroy(transform.parent.gameObject);
        }
    }
}