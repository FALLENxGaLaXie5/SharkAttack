using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMineSpawner : MonoBehaviour
{
    [SerializeField] GameObject minePrefab;
    [SerializeField] float mineSpawnWait = 2.21f;
    [SerializeField] float minTimeBetweenSpawns = 7f;
    [SerializeField] Transform spawnPosition;


    Animator animator;
    int lowerMineHash;
    float currentTimeAfterSpawn;
    bool mineSpawned = false;
    
    // Start is called before the first frame update
    void Start()
    {
        currentTimeAfterSpawn = minTimeBetweenSpawns;
        animator = transform.parent.GetComponentInChildren<Animator>();
        lowerMineHash = Animator.StringToHash("lowerMine");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMineSpawningTimer();
    }

    

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !mineSpawned)
        {
            mineSpawned = true;
            animator.SetTrigger(lowerMineHash);
            StartCoroutine(SpawnMine());
        }
    }

    IEnumerator SpawnMine()
    {
        yield return new WaitForSeconds(mineSpawnWait);
        Instantiate(minePrefab, spawnPosition.position, Quaternion.identity);
    }

    void UpdateMineSpawningTimer()
    {
        if (mineSpawned)
        {
            currentTimeAfterSpawn -= Time.deltaTime;
            if (currentTimeAfterSpawn <= 0)
            {
                mineSpawned = false;
                currentTimeAfterSpawn = minTimeBetweenSpawns;
            }
        }
    }

}
