/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteMove : MonoBehaviour
{
    [SerializeField] public SpawnCylindre spawnCylindre;
    [SerializeField] public EventZone eventZone;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        MeteoriteMoves();
    }
    public void SetObject(SpawnCylindre cylindre, EventZone zone)
    {
        spawnCylindre = cylindre;
        eventZone = zone;
    }
    void MeteoriteMoves()
    {
        rb.AddForce(1,0,0);
    }
}
*/