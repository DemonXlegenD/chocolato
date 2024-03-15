using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesGame : MonoBehaviour
{

    List<GameObject> enemiesList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddEnemy(GameObject enemy)
    {
        enemiesList.Add(enemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemiesList.Remove(enemy);
    }
}
