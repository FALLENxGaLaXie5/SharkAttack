using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBrain : MonoBehaviour
{
    // Start is called before the first frame update
    public float minTime = 1;
    public float maxTime = 3;
    
    float spawnTime = 0, totalTime;

    public GameObject topLeftBoundary, topRightBoundary, bottomLeftBoundary, bottomRightBoundary;

    public GameObject prefab;


    GameObject[,] boundaries;

    void Start()
    {
        spawnTime = totalTime + Random.Range(minTime, maxTime);
        boundaries = new GameObject[,]
        {
            { topLeftBoundary, bottomLeftBoundary },
            { topRightBoundary, bottomRightBoundary }
        };
    }

    // Update is called once per frame
    void Update()
    {
        totalTime += Time.deltaTime;
        if (totalTime > spawnTime)
        {
            Spawn();
        }
    }

    void Spawn()
    {
        
        int sideOfScreen = Random.Range(0, 2);
        float transformY = Random.Range(boundaries[sideOfScreen, 0].transform.position.y, boundaries[sideOfScreen, 1].transform.position.y);
        GameObject newObject = (GameObject)Instantiate(prefab, new Vector2(boundaries[sideOfScreen, 0].transform.position.x, transformY), transform.rotation);
        newObject.GetComponent<ISpawnable>().Spawn(sideOfScreen);
        spawnTime = totalTime + Random.Range(minTime - (minTime * GameManager.instance.spawnMultiplier), maxTime - (maxTime * GameManager.instance.spawnMultiplier));
    }
}
