using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteMove : MonoBehaviour
{
    
    [SerializeField] float finishDistanceTarget;
    [SerializeField] float moveSpeed;

    [SerializeField] public SpawnCylindre spawnCylindre;
    [SerializeField] public EventZone eventZone;

    // Update is called once per frame
    void Update()
    {
        MeteoriteMoves();
    }

    void MeteoriteMoves()
    {
        if (eventZone.GetActiveMeteore())
        {
            if (Vector3.Distance(transform.position, spawnCylindre.GetSpawnPosition()) <= 2f)
            {
                eventZone.ResetMeteorStatus();
            }
            else
            {
                Vector3 direction = (spawnCylindre.GetSpawnPosition() - transform.position).normalized;
                transform.Translate(direction * Time.deltaTime * 15, Space.World);
            }
        }
    }
}
