using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObjects : MonoBehaviour
{
    [SerializeField] int maxEnemies;
    [SerializeField] int maxenemyBullets;
    private List<GameObject> enemies;
    private List<GameObject> enemyBullets;
    [SerializeField] GameObject bulletPrefab;
    // Start is called before the first frame update
    void Start()
    {
        enemies = new List<GameObject>();
        enemyBullets = new List<GameObject>();
        for (int i = 0; i < maxenemyBullets; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            bullet.SetActive(false);
            enemyBullets.Add(bullet);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<GameObject> GetPoolenemyBullets()
    {
        return enemyBullets;
    }

    public List<GameObject> GetPoolEnemies()
    {
        return enemies;
    }

    public GameObject GetFreeEnemyBullet()
    {
        foreach (GameObject bullet in enemyBullets)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }
        return null;
    }

    public void SpawnEnemyBullet(Transform enemyTransform)
    {
        GameObject bullet = GetFreeEnemyBullet();
        if (bullet != null)
        {
            bullet.transform.position = enemyTransform.position;
            bullet.transform.rotation = enemyTransform.rotation;
            bullet.SetActive(true);
            bullet.GetComponent<Bullet>().StartBullet();
        }
    }
}
