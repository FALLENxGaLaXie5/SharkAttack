using Assets.Dasbor.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullBrain : MonoBehaviour, IBrain
{
    // Start is called before the first frame update
    public Vector3 direction;

    public float speed = 2f;

    // Update is called once per frame
    void Update()
    {
        transform.parent.position = Vector2.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction;
        if (direction.x < 0 && transform.parent.localScale.x > 0)
        {
            transform.parent.localScale = new Vector3(-transform.parent.localScale.x, transform.parent.localScale.y, transform.parent.localScale.z);
        }
        else if (direction.x > 0 && transform.parent.localScale.x < 0)
        {
            transform.parent.localScale = new Vector3(transform.parent.localScale.x * -1, transform.parent.localScale.y, transform.parent.localScale.z);
        }
    }

    public Vector2 GetDirection()
    {
        return direction;
    }
}
