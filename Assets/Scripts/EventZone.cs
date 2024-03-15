using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.FlowStateWidget;

public class EventZone : MonoBehaviour
{
    [SerializeField] private EventZone eventZone;
    [SerializeField] private SpawnCylindre spawnCylindre;

    [SerializeField] GameObject particuleFusion;
    [SerializeField] GameObject particuleStorm;
    [SerializeField] GameObject particuleAsteroid;
    [SerializeField] List<GameObject> fusionPool;

    private int randomAttack;

    [SerializeField] float finishDistanceTarget;
    [SerializeField] float moveSpeed;

    private bool canAttack = false;
    private bool canAttackMeteorite = false;
    private bool isCoroutineRunning = false;

    public Vector3 newPosition;

    //Getters
    public bool GetActiveMeteore() { return canAttackMeteorite; }

    //Setters
    public bool SetActiveMeteore(bool canAttackMeteorite) { return canAttackMeteorite; }

    void Start()
    {
        eventZone = GetComponent<EventZone>();

        randomAttack = Random.Range(0, 3);
    }

    private void Update()
    {
        if (canAttack && !isCoroutineRunning)
        {
            if (randomAttack == 0)
            {
                /*                GameObject typeAttack = particuleFusion;
                                StartCoroutine(StartTypeAttackToPlayer(typeAttack));*/
                canAttackMeteorite = true;
                newPosition = new Vector3(spawnCylindre.GetSpawnPosition().x, spawnCylindre.GetSpawnPosition().y + 20, spawnCylindre.GetSpawnPosition().z);
                fusionPool.Add(PoolManager.SpawnObject(particuleAsteroid.gameObject, newPosition, particuleAsteroid.transform.rotation));
                particuleAsteroid.GetComponentInChildren<MeteoriteMove>().eventZone = eventZone;
                particuleAsteroid.GetComponentInChildren<MeteoriteMove>().spawnCylindre = spawnCylindre;
            }
            else if (randomAttack == 1)
            {
                /*                GameObject typeAttack = particuleStorm;
                                StartCoroutine(StartTypeAttackToPlayer(typeAttack));*/
                canAttackMeteorite = true;
                newPosition = new Vector3(spawnCylindre.GetSpawnPosition().x, spawnCylindre.GetSpawnPosition().y + 20, spawnCylindre.GetSpawnPosition().z);
                fusionPool.Add(PoolManager.SpawnObject(particuleAsteroid.gameObject, newPosition, particuleAsteroid.transform.rotation));
                particuleAsteroid.GetComponentInChildren<MeteoriteMove>().eventZone = eventZone;
                particuleAsteroid.GetComponentInChildren<MeteoriteMove>().spawnCylindre = spawnCylindre;
            }
            else if (randomAttack == 2)
            {
                canAttackMeteorite = true;
                newPosition = new Vector3(spawnCylindre.GetSpawnPosition().x, spawnCylindre.GetSpawnPosition().y + 20, spawnCylindre.GetSpawnPosition().z);
                fusionPool.Add(PoolManager.SpawnObject(particuleAsteroid.gameObject, newPosition, particuleAsteroid.transform.rotation));
                particuleAsteroid.GetComponentInChildren<MeteoriteMove>().eventZone = eventZone;
                particuleAsteroid.GetComponentInChildren<MeteoriteMove>().spawnCylindre = spawnCylindre;
            }
            isCoroutineRunning = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (eventZone != null)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                canAttack = true;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        KillClone();
        StopAllCoroutines();
        canAttack = false;
        isCoroutineRunning = false;
    }

    IEnumerator StartTypeAttackToPlayer(GameObject typeAttack)
    {
        while (true)
        {
            fusionPool.Add(PoolManager.SpawnObject(typeAttack, spawnCylindre.GetSpawnPosition(), typeAttack.transform.rotation));

            yield return new WaitForSeconds(6f);

            KillClone();

            randomAttack = Random.Range(0, 3);
            isCoroutineRunning = false;
            yield break;
        }
    }

    private void StartTypeAttackToPlayer2(GameObject typeAttack)
    {
        if (canAttackMeteorite)
        {
            if (Vector3.Distance(typeAttack.transform.position, spawnCylindre.GetSpawnPosition()) <= finishDistanceTarget)
            {
                Debug.Log("arrivée");
                KillClone();
                randomAttack = Random.Range(0, 3);
                canAttackMeteorite = false;
            }
            else
            {
                Debug.Log("DESTINATION " + spawnCylindre.GetSpawnPosition());
                Debug.Log("ASTEROID " + typeAttack.transform.position);
                Vector3 direction = (spawnCylindre.GetSpawnPosition() - typeAttack.transform.position).normalized;
                typeAttack.transform.Translate(direction * Time.deltaTime * moveSpeed, Space.World);
            }
        }
    }

    private void KillClone()
    {
        foreach (var f in fusionPool)
        {
            PoolManager.ReturnObjectToPool(f);
        }
        fusionPool.Clear();
    }

    public void ResetMeteorStatus()
    {
        KillClone();
        randomAttack = Random.Range(0, 3);
        canAttackMeteorite = false;
    }
}
