using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventZone : MonoBehaviour
{
    [SerializeField] EventZone typeZone;
    [SerializeField] GameObject particuleFusion;
    [SerializeField] GameObject particuleStorm;
    [SerializeField] List<GameObject> fusionPool;

    public SpawnCylindre spawnCylindre;
    private int randomAttack;

    bool canAttack = false;
    bool isCoroutineRunning = false;

    void Start()
    {
        typeZone = GetComponent<EventZone>();

        randomAttack = Random.Range(0, 2);
    }

    private void Update()
    {
        if(canAttack && !isCoroutineRunning)
        {
            StartCoroutine(StartTypeAttackToPlayer());
            isCoroutineRunning = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (typeZone != null)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                canAttack = true;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        StopAllCoroutines();
        canAttack = false;
        isCoroutineRunning = false;
    }

    IEnumerator StartTypeAttackToPlayer()
    {
        while (true)
        {
            GameObject typeAttack = randomAttack == 0 ? particuleStorm : particuleFusion;

            fusionPool.Add(PoolManager.SpawnObject(typeAttack, spawnCylindre.GetSpawnPosition(), typeAttack.transform.rotation));

            yield return new WaitForSeconds(6f);

            foreach (var f in fusionPool)
            {
                PoolManager.ReturnObjectToPool(f);
            }
            fusionPool.Clear();

            randomAttack = Random.Range(0, 2);
            isCoroutineRunning = false;
            yield break;
        }
    }
}
