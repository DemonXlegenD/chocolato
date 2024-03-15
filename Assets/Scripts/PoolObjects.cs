using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObjects : MonoBehaviour
{
    [SerializeField] int maxEnemies;
    [SerializeField] int maxBullets;
    private List<GameObject> enemies;
    private List<GameObject> bullets;
    [SerializeField] GameObject bulletPrefab;
    // Start is called before the first frame update
    void Start()
    {
        enemies = new List<GameObject>();
        bullets = new List<GameObject>();
        for (int i = 0; i < maxBullets; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            bullet.SetActive(false);
            bullets.Add(bullet);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<GameObject> GetPoolBullets()
    {
        return bullets;
    }

    public List<GameObject> GetPoolEnemies()
    {
        return enemies;
    }

    public GameObject GetFreeBullet()
    {
        foreach (GameObject bullet in bullets)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }
        return null;
    }

    public void SpawnBullet(Transform enemyTransform)
    {
        GameObject bullet = GetFreeBullet();
        if (bullet != null)
        {
            bullet.transform.position = enemyTransform.position;
            bullet.transform.rotation = enemyTransform.rotation;
            bullet.SetActive(true);
            bullet.GetComponent<Bullet>().StartBullet();
        }
    }   
    public void SpawnBullet(Vector3 dir, Transform spawn)
    {
        GameObject bullet = GetFreeBullet();
        if (bullet != null)
        {
            bullet.transform.forward = dir;
            bullet.transform.position = spawn.position;
            bullet.SetActive(true);
            bullet.GetComponent<Bullet>().type = Bullet.BulletType.PlayerBullet;
            bullet.GetComponent<Bullet>().StartBullet();
        }
    }
}
