using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteMove : MonoBehaviour
{
    [SerializeField] MeshRenderer particuleAsteroidMesh;
    [SerializeField] TrailRenderer particuleAsteroidTrail;
    [SerializeField] public EventZone eventZone;
    [SerializeField] public SpawnCylindre spawnCylindre;
    
    [SerializeField] Material whiteMaterial;
    [SerializeField] Material blackMaterial;

    private void Awake()
    {
        particuleAsteroidMesh = GetComponentInChildren<MeshRenderer>();
        particuleAsteroidMesh.material = blackMaterial;

        particuleAsteroidTrail = GetComponentInChildren<TrailRenderer>();
        particuleAsteroidTrail.material = blackMaterial;
    }

    private void Start()
    {

    }

    public void SetObject(SpawnCylindre cylindre, EventZone zone)
    {
        spawnCylindre = cylindre;
        eventZone = zone;
    }

    private void Update()
    {
        if (eventZone.GetActiveNameZone() != null)
        {
            if (eventZone.GetActiveNameZone() == "BlackZoneChocolate")
            {
                Debug.Log("good black normalement");
                particuleAsteroidMesh.material = whiteMaterial;
                particuleAsteroidTrail.material = whiteMaterial;
            }
            else if (eventZone.GetActiveNameZone() == "WhiteZoneChocolate")
            {
                Debug.Log("good white normalement");
                particuleAsteroidMesh.material = blackMaterial;
                particuleAsteroidTrail.material= blackMaterial;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // si collision alors détruit la zone
        this.gameObject.SetActive(false);
    }
}
