using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    static WaveSpawner instance;

    #region Singleton
    public static WaveSpawner GetInstance()
    {
        if(instance == null)
            return instance = FindAnyObjectByType<WaveSpawner>();
        else return instance;
    }
    #endregion




    [SerializeField] PoolObjects poolObjects;
    public float spawnRate = 1f;
    public float timeBetweenWaves = 10f;
    public GameObject enemy;
    public int enemyCount;
    public float i;
    bool waveIsDone = true;
    


    public void Init()
    {
        poolObjects = PoolObjects.GetInstance();
    }
    
    void Update()
    {
        if (waveIsDone == true)
        {
            StartCoroutine(waveSpawner());
        }
    }

    IEnumerator waveSpawner()
    {
        for (i = 0; i < enemyCount; i++)
        {
            GameObject enemyClone = Instantiate(enemy);

            yield return new WaitForSeconds(spawnRate);
        }

        spawnRate -= 0.1f;

        yield return new WaitForSeconds(timeBetweenWaves);

        waveIsDone = true;
    }
}
