using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventZone : MonoBehaviour
{
    [SerializeField] private EventZone eventZone;
    [SerializeField] private SpawnCylindre spawnCylindre;
    [SerializeField] private GameObject rbPlayer;

    [SerializeField] GameObject particuleFusion;
    [SerializeField] GameObject particuleStorm;
    //[SerializeField] GameObject particuleAsteroid;
    [SerializeField] List<GameObject> fusionPool;

    private int randomAttack;

    [SerializeField] float finishDistanceTarget;
    [SerializeField] float moveSpeed;

    private bool canAttack = false;
    private bool isCoroutineRunning = false;

    private string stockNameZone;

    //Getters
    public string GetActiveNameZone() { return stockNameZone; }

    //Setters
    public string SetActiveNameZone (string stockNameZone) { return stockNameZone; }

    //private Vector3 newPosition;

    void Start()
    {
        eventZone = GetComponent<EventZone>();

        randomAttack = Random.Range(0, 3);

        //particuleAsteroid.GetComponentInChildren<MeteoriteMove>().eventZone = eventZone;
    }

    private void Update()
    {
        SetActiveNameZone(eventZone.gameObject.name);
        if (canAttack && !isCoroutineRunning)
        {
            if (randomAttack == 0)
            {
                GameObject typeAttack = particuleFusion;
                StartCoroutine(StartTypeAttackToPlayer(typeAttack));
            }
            else if (randomAttack == 1)
            {
                GameObject typeAttack = particuleStorm;
                StartCoroutine(StartTypeAttackToPlayer(typeAttack));
            }
           /* else if (randomAttack == 2)
            {
                // pour la météorite on set un Y supérieur pour qu'elle parte du dessus
                newPosition = new Vector3(spawnCylindre.GetSpawnPosition().x, 20f, spawnCylindre.GetSpawnPosition().z);
                var tempMeteor = PoolManager.SpawnObject(particuleAsteroid.gameObject, newPosition, particuleAsteroid.transform.rotation);
                fusionPool.Add(tempMeteor);
                tempMeteor.GetComponent<MeteoriteMove>().SetObject(spawnCylindre, eventZone);
                particuleAsteroid.GetComponentInChildren<MeteoriteMove>().eventZone = eventZone;
                particuleAsteroid.GetComponentInChildren<MeteoriteMove>().spawnCylindre = spawnCylindre;
            }*/
            isCoroutineRunning = true;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            stockNameZone = this.gameObject.name;
            SetActiveNameZone(stockNameZone);
            if (eventZone != null)
            {
                if (other.gameObject.tag == "Player")
                {
                    canAttack = true;
                }
               /* if (other.gameObject.tag == "Asteroid")
                {
                    DisplayClone();
                    // Destroy la zone de chocolat en question
                }*/
            }
        }
       
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            DisplayClone();
            StopAllCoroutines();
            canAttack = false;
            isCoroutineRunning = false;
        }
    }


    IEnumerator StartTypeAttackToPlayer(GameObject typeAttack)
    {
        fusionPool.Add(PoolManager.SpawnObject(typeAttack, spawnCylindre.GetSpawnPosition(), typeAttack.transform.rotation));

        yield return new WaitForSeconds(6f);

        DisplayClone();
        randomAttack = Random.Range(0, 3);
        isCoroutineRunning = false;
    }

    private void DisplayClone()
    {
        foreach (var f in fusionPool)
        {
            PoolManager.ReturnObjectToPool(f);
        }
    }
}
