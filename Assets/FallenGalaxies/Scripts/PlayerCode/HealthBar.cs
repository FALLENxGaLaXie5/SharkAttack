using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    Transform bar;

    // Start is called before the first frame update
    void Start()
    {
        bar = transform.Find("Bar");
    }

    public void SetSize(float sizeNormalized)
    {
        bar.localScale = new Vector3(sizeNormalized, 1f);
    }

    public void SetColor(Color color)
    {
        bar.Find("BarSprite").GetComponent<SpriteRenderer>().color = color;
        if (color == Color.white)
        {
            print("I set color to white!");
        }
    }
}
