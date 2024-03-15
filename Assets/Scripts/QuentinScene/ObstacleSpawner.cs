using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public LayerMask groundLayer;
    public float spawnRadius = 5f;
    public float minDistanceBetweenObstacles = 2f;

    public GameObject[] obstaclePrefabs;
    public Collider[] forbiddenZones;
    public float bufferDistance = 1.5f;

    public int maxTries = 1000;

    void Start()
    {
    }

    public GameObject SpawnObstacles(Vector2 chunkMin, Vector2 chunkMax, GameObject meshObject)
    {
        int tries = 0;

        while (tries < maxTries)
        {
            Vector3 randomPoint = GenerateRandomPoint(chunkMin, chunkMax);
            if (CheckObstaclePlacement(randomPoint))
            {

                GameObject newObstacle = Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)], randomPoint, Quaternion.identity);
                newObstacle.transform.localScale = Vector3.one * Random.Range(2, 15);
                    
                newObstacle.transform.parent = meshObject.transform;

                return newObstacle;
            }
            tries++;
        }
        return null;
    }

    Vector3 GenerateRandomPoint(Vector2 chunkMin, Vector2 chunkMax)
    {
        Vector3 randomPoint = new Vector3(Random.Range(chunkMin.x, chunkMax.x), 0, Random.Range(chunkMin.y, chunkMax.y));
        while (randomPoint.y > 160)
        {
            randomPoint = new Vector3(Random.Range(chunkMin.x, chunkMax.x), 0, Random.Range(chunkMin.y, chunkMax.y));
        }
        Debug.Log("Random " +randomPoint);
        return randomPoint;
    }

    bool CheckObstaclePlacement(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, spawnRadius);

        // Vérifier si l'emplacement est déjà occupé par un obstacle
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Obstacle"))
            {
                return false;
            }
        }
        return true;
    }

    bool IsInForbiddenZone(Vector3 position)
    {
        foreach (Collider zone in forbiddenZones)
        {
            Bounds zoneBounds = zone.bounds;
            zoneBounds.Expand(bufferDistance);
            if (zone.bounds.Contains(position))
            {
                return true;
            }
        }
        return false;
    }

}