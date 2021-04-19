using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBrainFish : MonoBehaviour
{
    [SerializeField] bool testPowerups = false;
    [SerializeField] bool testOnlyHealth = false;
    [SerializeField] bool testOnlyStamina = false;
    [SerializeField] bool testOnlyShield = false;
    // Start is called before the first frame update
    public float minTime = 1;
    public float maxTime = 3;

    float spawnTime = 0, totalTime;

    public GameObject topLeftBoundary, topRightBoundary, bottomLeftBoundary, bottomRightBoundary;

    public GameObject prefab;

    [SerializeField] GameObject healthPowerupPrefab;
    [SerializeField] GameObject staminaPowerupPrefab;
    [SerializeField] GameObject shieldPowerupPrefab;
    [SerializeField] GameObject bombPowerupPrefab;


    GameObject[,] boundaries;

    #region Unity Event Functions

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

    #endregion

    #region Class Functions
    void Spawn()
    {

        int sideOfScreen = Random.Range(0, 2);
        float transformY = Random.Range(boundaries[sideOfScreen, 0].transform.position.y, boundaries[sideOfScreen, 1].transform.position.y);
        GameObject newObject = (GameObject)Instantiate(spawnObject(), new Vector2(boundaries[sideOfScreen, 0].transform.position.x, transformY), transform.rotation);
        newObject.GetComponent<ISpawnable>().Spawn(sideOfScreen);
        spawnTime = totalTime + Random.Range(minTime - (minTime * GameManager.instance.spawnMultiplier), maxTime - (maxTime * GameManager.instance.spawnMultiplier));
    }

    GameObject spawnObject()
    {
        int type = Random.Range(0, 11);
        if (!testPowerups)
        {
            return switchFishSpawn(type);
        }
        else
        {
            return powerupsFishTestFunction(type);
        }
    }

    GameObject powerupsFishTestFunction(int type)
    {
        if (testOnlyHealth)
        {
            return healthPowerupPrefab;
        }
        else if (testOnlyStamina)
        {
            return staminaPowerupPrefab;
        }
        else if (testOnlyShield)
        {
            return shieldPowerupPrefab;
        }
        else
        {
            switch (type)
            {
                case 1:
                    return healthPowerupPrefab;
                case 2:
                    return healthPowerupPrefab;
                case 3:
                    return healthPowerupPrefab;
                case 4:
                    return staminaPowerupPrefab;
                case 5:
                    return staminaPowerupPrefab;
                case 6:
                    return staminaPowerupPrefab;
                case 7:
                    return staminaPowerupPrefab;
                case 8:
                    return shieldPowerupPrefab;
                case 9:
                    return shieldPowerupPrefab;
                case 10:
                    return shieldPowerupPrefab;
                case 11:
                    return shieldPowerupPrefab;
                default:
                    return healthPowerupPrefab;
            }
        }
        
    }

    GameObject switchFishSpawn(int type)
    {
        switch (type)
        {            
            case 7:
                return healthPowerupPrefab;
            case 8:
                return staminaPowerupPrefab;
            case 9:
                return shieldPowerupPrefab;
            case 10:
                return bombPowerupPrefab;
            default:
                return prefab;            
        }
    }
    
    #endregion
}
