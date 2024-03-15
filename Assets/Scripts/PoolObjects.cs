using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PoolObjects : MonoBehaviour
{
    [SerializeField] int maxCookies;
    [SerializeField] int maxBasicEnemy;
    [SerializeField] int maxRangedEnemy;
    [SerializeField] int maxKamikazeEnemy;
    [SerializeField] int maxDiggerEnemy;
    [SerializeField] int maxenemyBullets;
    private List<GameObject> enemyBullets;
    private List<GameObject> lootCookiesBlack;
    private List<GameObject> lootCookiesWhite;
    private List<GameObject> basicEnemies;
    private List<GameObject> rangedEnemies;
    private List<GameObject> kamikazeEnemies;
    private List<GameObject> diggerEnemies;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject cookieBlackPrefab;
    [SerializeField] GameObject cookieWhitePrefab;
    [SerializeField] GameObject basicWhite;
    [SerializeField] GameObject basicBlack;
    [SerializeField] GameObject rangedWhite;
    [SerializeField] GameObject rangedBlack;
    [SerializeField] GameObject kamikazeWhite;
    [SerializeField] GameObject kamikazeBlack;
    [SerializeField] GameObject diggerWhite;
    [SerializeField] GameObject diggerBlack;

    [Header("Wave")]
    [SerializeField] int totalEnemiesWave1;
    // Start is called before the first frame update
    void Start()
    {
        enemyBullets = new List<GameObject>();
        lootCookiesBlack = new List<GameObject>();
        lootCookiesWhite = new List<GameObject>();
        basicEnemies = new List<GameObject>();
        rangedEnemies = new List<GameObject>();
        kamikazeEnemies = new List<GameObject>();
        diggerEnemies = new List<GameObject>();

        for (int i = 0; i < maxenemyBullets; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            bullet.SetActive(false);
            enemyBullets.Add(bullet);
        }
        for (int i = 0; i < maxCookies; i++)
        {
            if (i < maxCookies / 2)
            {
                GameObject cookie = Instantiate(cookieBlackPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                cookie.SetActive(false);
                lootCookiesBlack.Add(cookie);
            }
            else
            {
                GameObject cookie = Instantiate(cookieWhitePrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                cookie.SetActive(false);
                lootCookiesWhite.Add(cookie);
            }
        }
        for (int i = 0; i < maxBasicEnemy; i++)
        {
            if (i < maxBasicEnemy / 2)
            {
                GameObject basicEnemy = Instantiate(basicWhite, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                basicEnemy.SetActive(false);
                basicEnemies.Add(basicEnemy);
            }
            else
            {
                GameObject basicEnemy = Instantiate(basicBlack, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                basicEnemy.SetActive(false);
                basicEnemies.Add(basicEnemy);
            }
        }
        for (int i = 0; i < maxRangedEnemy; i++)
        {
            if (i < maxRangedEnemy/2)
            {
                GameObject rangedEnemy = Instantiate(rangedWhite, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                rangedEnemy.SetActive(false);
                rangedEnemies.Add(rangedEnemy);

            }
            else
            {
                GameObject rangedEnemy = Instantiate(rangedBlack, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                rangedEnemy.SetActive(false);
                rangedEnemies.Add(rangedEnemy);
            }
        }
        for (int i = 0; i < maxKamikazeEnemy; i++)
        {
            if (i < maxKamikazeEnemy/2)
            {
                GameObject kamikazeEnemy = Instantiate(kamikazeWhite, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                kamikazeEnemy.SetActive(false);
                kamikazeEnemies.Add(kamikazeEnemy);

            }
            else 
            {
                GameObject kamikazeEnemy = Instantiate(kamikazeBlack, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                kamikazeEnemy.SetActive(false);
                kamikazeEnemies.Add(kamikazeEnemy);
            }
        }
        for (int i = 0; i < maxDiggerEnemy; i++)
        {
            if (i < maxDiggerEnemy / 2)
            {
                GameObject diggerEnemy = Instantiate(diggerWhite, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                diggerEnemy.SetActive(false);
                diggerEnemies.Add(diggerEnemy);

            }
            else 
            {
                GameObject diggerEnemy = Instantiate(diggerBlack, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                diggerEnemy.SetActive(false);
                diggerEnemies.Add(diggerEnemy);
            }
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

    public List<GameObject> GetPoolLootCookiesBlack()
    {
        return lootCookiesBlack;
    }

    public List<GameObject> GetPoolLootCookiesWhite()
    {
        return lootCookiesWhite;
    }




    [System.Serializable]
    public class EnemyDeathEvent : UnityEvent<int>
    {
    }




    public GameObject GetFreeBullet()
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


    public GameObject GetFreeLootCookiesWhite()
    {
        foreach (GameObject cookie in lootCookiesWhite)
        {
            if (!cookie.activeInHierarchy)
            {
                return cookie;
            }
        }
        return null;
    }

    public GameObject GetFreeLootCookiesBlack()
    {
        foreach (GameObject cookie in lootCookiesBlack)
        {
            if (!cookie.activeInHierarchy)
            {
                return cookie;
            }
        }
        return null;
    }

    public GameObject GetFreeBasicWhite()
    {
        for(int i = 0; i < maxBasicEnemy/2; i++)
        {
            if (basicEnemies[i] != null && basicEnemies[i].activeInHierarchy) {
                return basicEnemies[i];
            }
        }
        return null;
    }

    public GameObject GetFreeBasicBlack()
    {
        for (int i = maxBasicEnemy / 2; i < maxBasicEnemy; i++)
        {
            if (basicEnemies[i] != null && basicEnemies[i].activeInHierarchy)
            {
                return basicEnemies[i];
            }
        }
        return null;
    }

    public GameObject GetFreeRangedWhite()
    {
        for (int i = 0; i < maxRangedEnemy / 2; i++)
        {
            if (rangedEnemies[i] != null && rangedEnemies[i].activeInHierarchy)
            {
                return rangedEnemies[i];
            }
        }
        return null;
    }

    public GameObject GetFreeRangedBlack()
    {
        for (int i = maxRangedEnemy / 2; i < maxRangedEnemy; i++)
        {
            if (rangedEnemies[i] != null && rangedEnemies[i].activeInHierarchy)
            {
                return rangedEnemies[i];
            }
        }
        return null;
    }

    public GameObject GetFreeKamikazeWhite()
    {
        for (int i = 0; i < maxKamikazeEnemy / 2; i++)
        {
            if (kamikazeEnemies[i] != null && kamikazeEnemies[i].activeInHierarchy)
            {
                return kamikazeEnemies[i];
            }
        }
        return null;
    }

    public GameObject GetFreeKamikazeBlack()
    {
        for (int i = maxKamikazeEnemy / 2; i < maxKamikazeEnemy; i++)
        {
            if (kamikazeEnemies[i] != null && kamikazeEnemies[i].activeInHierarchy)
            {
                return kamikazeEnemies[i];
            }
        }
        return null;
    }

    public GameObject GetFreeDiggerWhite()
    {
        for (int i = 0; i < maxDiggerEnemy / 2; i++)
        {
            if (diggerEnemies[i] != null && diggerEnemies[i].activeInHierarchy)
            {
                return diggerEnemies[i];
            }
        }
        return null;
    }

    public GameObject GetFreeDiggerBlack()
    {
        for (int i = maxDiggerEnemy / 2; i < maxDiggerEnemy; i++)
        {
            if (diggerEnemies[i] != null && diggerEnemies[i].activeInHierarchy)
            {
                return diggerEnemies[i];
            }
        }
        return null;
    }


    public void SpawnEnemyBullet(Transform enemyTransform)
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

    public void SpawnCookieWhite(Transform enemyTransform)
    {
        GameObject cookie = GetFreeLootCookiesWhite();
        if (cookie != null)
        {
            cookie.transform.position = enemyTransform.position;
            cookie.SetActive(true);
        }
    }

    public void SpawnCookieBlack(Transform enemyTransform)
    {
        GameObject cookie = GetFreeLootCookiesBlack();
        if (cookie != null)
        {
            cookie.transform.position = new Vector3(enemyTransform.position.x,enemyTransform.position.y+0.5f,enemyTransform.position.z);
            cookie.SetActive(true);
        }
    }

    public void StartWave1()
    {
        for(int i = 0; i < totalEnemiesWave1; i++)
        {
            
        }
    }
}
