using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerper : MonoBehaviour
{
    float startX;
    [Tooltip("Distance Travelled")] [Range(10f, 500f)] [SerializeField] float endX = 200;

    float currentTime = 0f;
    float maxTime = 5f;
    // Start is called before the first frame update
    void Start()
    {

        startX = 0;
        endX = startX + 200f;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        float percPassed = currentTime / maxTime;
        float newX = Mathf.Lerp(startX, endX, percPassed);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
