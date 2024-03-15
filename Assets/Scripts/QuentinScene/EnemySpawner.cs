using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeChocolate
{

}
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private TypeChocolate typeChocolate;

    [SerializeField] private GameObject enemy;

    [SerializeField] private float SpawnDelay;

    private EnemiesGame enemiesGame;

    bool spawnIsStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        enemiesGame = FindObjectOfType<EnemiesGame>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawnIsStarted)
        {
            StartCoroutine(SpawnEnemy(SpawnDelay));
        }
        
    }

    IEnumerator SpawnEnemy(float time)
    {
        spawnIsStarted = true;

        yield return new WaitForSeconds(time);

        GameObject newEnemy = Instantiate(enemy);
        newEnemy.transform.SetParent(enemiesGame.transform, false);
        enemiesGame.AddEnemy(newEnemy);

        spawnIsStarted = false;
    }
}
