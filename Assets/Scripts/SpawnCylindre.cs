using UnityEngine;

public class SpawnCylindre : MonoBehaviour
{
    public Transform cylinderTransform;

    Vector3 GetRandomSpawnPosition()
    {
        float radius = cylinderTransform.localScale.x / 2f;
        float height = cylinderTransform.localScale.y;

        float angle = Random.Range(0f, Mathf.PI * 2f);
        float h = Random.Range(0f, height);
        float r = Random.Range(0f, radius);

        float x = Mathf.Cos(angle) * r;
        float z = Mathf.Sin(angle) * r;

        return cylinderTransform.position + new Vector3(x, h, z);
    }

    bool IsInsideCylinder(Vector3 position)
    {
        float radius = cylinderTransform.localScale.x / 2f;
        float height = cylinderTransform.localScale.y;

        float distFromCenter = Vector2.Distance(new Vector2(position.x, position.z), new Vector2(cylinderTransform.position.x, cylinderTransform.position.z));
        return distFromCenter <= radius && position.y <= height + cylinderTransform.position.y && position.y >= cylinderTransform.position.y;
    }

    Vector3 FindValidSpawnPosition()
    {
        Vector3 spawnPosition = Vector3.zero;
        spawnPosition = GetRandomSpawnPosition();

        if (IsInsideCylinder(spawnPosition))
        {
            return spawnPosition;
        }

        return Vector3.zero;
    }

    public Vector3 GetSpawnPosition()
    {
        return FindValidSpawnPosition();
    }

    void Start()
    {
        Debug.Log(GetSpawnPosition());
        GetSpawnPosition();
    }
}
