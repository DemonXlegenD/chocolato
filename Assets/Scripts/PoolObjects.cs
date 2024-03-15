using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PoolObjects : MonoBehaviour
{
    [SerializeField] int maxEnemies;
    [SerializeField] int maxCookies;
    [SerializeField] int maxVague1;
    [SerializeField] int maxVague2;
    [SerializeField] int maxVague3;
    [SerializeField] int maxVague4;
    [SerializeField] int maxenemyBullets;
    private List<GameObject> enemies;
    private List<GameObject> enemyBullets;
    private List<GameObject> lootCookiesBlack;
    private List<GameObject> lootCookiesWhite;
    private List<GameObject> vague1;
    private List<GameObject> vague2;
    private List<GameObject> vague3;
    private List<GameObject> vague4;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject cookieBlackPrefab;
    [SerializeField] GameObject cookieWhitePrefab;
    [SerializeField] GameObject basicWhite;
    [SerializeField] GameObject basicBlack;
    [SerializeField] GameObject regnedWhite;
    [SerializeField] GameObject regnedBlack;
    [SerializeField] GameObject kamikazeWhite;
    [SerializeField] GameObject kamikazeBlack;
    [SerializeField] GameObject diggerWhite;
    [SerializeField] GameObject diggerBlack;
    // Start is called before the first frame update
    void Start()
    {
        enemies = new List<GameObject>();
        enemyBullets = new List<GameObject>();
        lootCookiesBlack = new List<GameObject>();
        lootCookiesWhite = new List<GameObject>();
        lootCookiesWhite = new List<GameObject>();
        lootCookiesWhite = new List<GameObject>();
        lootCookiesWhite = new List<GameObject>();

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
        for (int i = 0; i < maxVague1; i++)
        {
            if (i < maxVague1 / 2)
            {
                GameObject enemiVague1 = Instantiate(basicWhite, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                enemiVague1.SetActive(false);
                vague1.Add(enemiVague1);
            }
            else
            {
                GameObject enemiVague1 = Instantiate(basicBlack, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                enemiVague1.SetActive(false);
                vague1.Add(enemiVague1);
            }
        }
        for (int i = 0; i < maxVague2; i++)
        {
            int tmp = maxVague2 / 4;
            if (i < tmp)
            {
                GameObject enemiVague1 = Instantiate(basicWhite, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                enemiVague1.SetActive(false);
                vague1.Add(enemiVague1);

            }
            else if (i < tmp * 2)
            {
                GameObject enemiVague1 = Instantiate(basicBlack, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                enemiVague1.SetActive(false);
                vague1.Add(enemiVague1);
            }
            else if (i < tmp * 3)
            {
                GameObject enemiVague1 = Instantiate(regnedWhite, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                enemiVague1.SetActive(false);
                vague1.Add(enemiVague1);
            }

            else
            {
                GameObject enemiVague1 = Instantiate(regnedBlack, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                enemiVague1.SetActive(false);
                vague1.Add(enemiVague1);
            }
        }
        for (int i = 0; i < maxVague3; i++)
        {
            int tmp = maxVague3 / 4;
            if (i < tmp)
            {
                GameObject enemiVague1 = Instantiate(basicWhite, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                enemiVague1.SetActive(false);
                vague1.Add(enemiVague1);

            }
            else if (i < tmp * 2)
            {
                GameObject enemiVague1 = Instantiate(basicBlack, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                enemiVague1.SetActive(false);
                vague1.Add(enemiVague1);
            }
            else if (i < tmp * 3)
            {
                GameObject enemiVague1 = Instantiate(regnedWhite, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                enemiVague1.SetActive(false);
                vague1.Add(enemiVague1);
            }

            else
            {
                GameObject enemiVague1 = Instantiate(regnedBlack, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                enemiVague1.SetActive(false);
                vague1.Add(enemiVague1);
            }
        }
        for (int i = 0; i < maxVague4; i++)
        {
            int tmp = maxVague4 / 4;
            if (i < tmp)
            {
                GameObject enemiVague1 = Instantiate(basicWhite, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                enemiVague1.SetActive(false);
                vague1.Add(enemiVague1);

            }
            else if (i < tmp * 2)
            {
                GameObject enemiVague1 = Instantiate(basicBlack, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                enemiVague1.SetActive(false);
                vague1.Add(enemiVague1);
            }
            else if (i < tmp * 3)
            {
                GameObject enemiVague1 = Instantiate(regnedWhite, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                enemiVague1.SetActive(false);
                vague1.Add(enemiVague1);
            }

            else
            {
                GameObject enemiVague1 = Instantiate(regnedBlack, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                enemiVague1.SetActive(false);
                vague1.Add(enemiVague1);
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
            cookie.transform.position = enemyTransform.position;
            cookie.SetActive(true);
        }
    }
}
