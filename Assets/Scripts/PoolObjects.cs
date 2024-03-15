using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PoolObjects : MonoBehaviour
{
    static PoolObjects instance;
    public static PoolObjects GetInstance()
    {
        if (instance == null)
            return instance = FindAnyObjectByType<PoolObjects>();
        else return instance;
    }


    [SerializeField] int maxEnemies;
    [SerializeField] int maxCookies;
    [SerializeField] int maxenemyBullets;
    private List<GameObject> enemies;
    private List<GameObject> enemyBullets;
    private List<GameObject> lootCookiesBlack;
    private List<GameObject> lootCookiesWhite;
    [Header("White Entities")]
    [SerializeField] GameObject enemyWhiteNormal;
    [SerializeField] GameObject enemyWhiteKamikaze;
    [SerializeField] GameObject enemyWhiteRanged;
    [SerializeField] GameObject enemyWhiteDigger;
    [Header("Black Entities")]
    [SerializeField] GameObject enemyBlackNormal;
    [SerializeField] GameObject enemyBlackKamikaze;
    [SerializeField] GameObject enemyBlackRanged;
    [SerializeField] GameObject enemyBlackDigger;

    [Header("Le reste")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject cookieBlackPrefab;
    [SerializeField] GameObject cookieWhitePrefab;
    // Start is called before the first frame update
    void Start()
    {
        enemies = new List<GameObject>();
        enemyBullets = new List<GameObject>();
        lootCookiesBlack = new List<GameObject>();
        lootCookiesWhite = new List<GameObject>();
        for (int i = 0; i < maxenemyBullets; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            bullet.SetActive(false);
            enemyBullets.Add(bullet);
        }
        for (int i = 0; i < maxCookies; i++)
        {
            if(i < maxCookies/2) 
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
    }

    public List<GameObject> GetPoolenemyBullets()
    {
        return enemyBullets;
    }

    public List<GameObject> GetPoolEnemies()
    {
        return enemies;
    }

    public List<GameObject> GetPoolLootCookiesBlack()
    {
        return lootCookiesBlack;
    }

    public List<GameObject> GetPoolLootCookiesWhite()
    {
        return lootCookiesWhite;
    }

    public GameObject GetFreeEnemy()
    {
        foreach (GameObject enemy in enemies) 
        {
            if(!enemy.activeInHierarchy)
                return enemy;
        }
        GameObject tempEnemy = Instantiate(bulletPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        tempEnemy.SetActive(false);
        enemyBullets.Add(tempEnemy);
        return tempEnemy;
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
        GameObject tempBullet = Instantiate(bulletPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        tempBullet.SetActive(false);
        enemyBullets.Add(tempBullet);
        return tempBullet;
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
        GameObject tempCookie = Instantiate(cookieWhitePrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        tempCookie.SetActive(false);
        lootCookiesWhite.Add(tempCookie);
        return tempCookie;
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
        GameObject tempCookie = Instantiate(cookieBlackPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        tempCookie.SetActive(false);
        lootCookiesBlack.Add(tempCookie);
        return tempCookie;
    }


    public void SpawnEnemy(Transform transform)
    {
        GameObject enemy = GetFreeEnemy();
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
}
