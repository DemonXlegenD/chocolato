using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    [SerializeField] Transform obstacle;
    readonly List<Transform> obstacleList = new List<Transform>();
    Vector3 position;
    [SerializeField] private int obstacleNB;
    [SerializeField] private int distance;
    [SerializeField] private int positionY;
    public void Generate(Vector2 minChunk, Vector2 maxChunk)
    {
        Debug.Log("vuu");
        position = new Vector3(10, positionY, 10);
        Transform item = Instantiate<Transform>(obstacle, position, Quaternion.identity);
        obstacleList.Add(item);

        for (int i = 0; i < obstacleNB; i++)
        {
            bool isValidPosition = false;

            for (int attempts = 0; attempts < 100; attempts++)
            {
                position = new Vector3(Random.Range(minChunk.x, maxChunk.x), positionY, Random.Range(minChunk.y, maxChunk.y));
                isValidPosition = true;

                foreach (Transform obstaclePosition in obstacleList)
                {
                    if (Vector3.Distance(position, obstaclePosition.position) < distance)
                    {
                        isValidPosition = false;
                        break;
                    }
                }
            }
            if (isValidPosition)
            {
                item = Instantiate<Transform>(obstacle, position, Quaternion.identity);
                obstacleList.Add(item);

            }
        }
    }
}
