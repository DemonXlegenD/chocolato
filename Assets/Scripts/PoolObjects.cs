using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PoolObjects : MonoBehaviour
{
    [Header("Max Pool")]
    [SerializeField] int maxCookies;
    [SerializeField] int maxBasicEnemy;
    [SerializeField] int maxRangedEnemy;
    [SerializeField] int maxKamikazeEnemy;
    [SerializeField] int maxDiggerEnemy;
    [SerializeField] int maxEnemyBullets;
    [SerializeField] int maxAreaEffect;
    [SerializeField] int maxPlayerBullets;
    [SerializeField] int maxBossBullets;

    [Header("Pools")]
    private List<GameObject> enemyBullets;
    private List<GameObject> playerBullets;
    private List<GameObject> lootCookiesBlack;
    private List<GameObject> lootCookiesWhite;
    private List<GameObject> basicEnemies;
    private List<GameObject> rangedEnemies;
    private List<GameObject> kamikazeEnemies;
    private List<GameObject> diggerEnemies;
    private List<GameObject> areaEffects;
    private List<GameObject> bossBullets;
    private GameObject boss;

    [Header("Prefabs")]
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
    [SerializeField] GameObject areaEffectPrefab;
    [SerializeField] GameObject bossFinal;

    [Header("Waves")]
    [SerializeField] int totalEnemiesWave1;
    [SerializeField] int totalEnemiesWave2;
    [SerializeField] int totalEnemiesWave3;
    [SerializeField] int totalEnemiesWave4;

    // Start is called before the first frame update
    void Awake()
    {
        enemyBullets = new List<GameObject>();
        playerBullets = new List<GameObject>();
        lootCookiesBlack = new List<GameObject>();
        lootCookiesWhite = new List<GameObject>();
        basicEnemies = new List<GameObject>();
        rangedEnemies = new List<GameObject>();
        kamikazeEnemies = new List<GameObject>();
        diggerEnemies = new List<GameObject>();
        areaEffects = new List<GameObject>();
        bossBullets = new List<GameObject>();

        for (int i = 0; i < maxEnemyBullets; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            bullet.SetActive(false);
            enemyBullets.Add(bullet);
        }
        for (int i = 0; i < maxPlayerBullets; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            bullet.SetActive(false);
            playerBullets.Add(bullet);
        }
        for (int i = 0; i < maxBossBullets; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            bullet.SetActive(false);
            bullet.transform.localScale = new Vector3(4, 4, 4);
            bossBullets.Add(bullet);
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
        for(int i = 0; i < maxAreaEffect; i++)
        {
            GameObject areaEffect = Instantiate(areaEffectPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            areaEffect.SetActive(false);
            areaEffects.Add(areaEffect);
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
        boss = Instantiate(bossFinal, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        boss.SetActive(false);
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


    public GameObject GetPoolBoss()
    { 
        return boss;
    }




    [System.Serializable]
    public class EnemyDeathEvent : UnityEvent<int>
    {
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

    public GameObject GetFreePlayerBullet()
    {
        foreach (GameObject bullet in playerBullets)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }
        return null;
    }

    public GameObject GetFreeBossBullet()
    {
        foreach (GameObject bullet in bossBullets)
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
        foreach (GameObject basicEnemy in basicEnemies)
        {
            if (!basicEnemy.activeInHierarchy && basicEnemy.GetComponent<EnemyBehaviour>().enemyColor == EnemyBehaviour.EnemyColor.chocoWhite)
            {
                basicEnemy.GetComponent<EnemyBehaviour>().ResetEnemy();
                return basicEnemy;
            }
        }
        return null;
    }

    public GameObject GetFreeBasicBlack()
    {
        foreach (GameObject basicEnemy in basicEnemies)
        {
            if (!basicEnemy.activeInHierarchy && basicEnemy.GetComponent<EnemyBehaviour>().enemyColor == EnemyBehaviour.EnemyColor.chocoBlack)
            {
                basicEnemy.GetComponent<EnemyBehaviour>().ResetEnemy();
                return basicEnemy;
            }
        }
        return null;
    }

    public GameObject GetFreeRangedWhite()
    {
        foreach (GameObject rangedEnemy in rangedEnemies)
        {
            if (!rangedEnemy.activeInHierarchy && rangedEnemy.GetComponent<EnemyBehaviour>().enemyColor == EnemyBehaviour.EnemyColor.chocoWhite)
            {
                rangedEnemy.GetComponent<EnemyBehaviour>().ResetEnemy();
                return rangedEnemy;
            }
        }
        return null;
    }

    public GameObject GetFreeRangedBlack()
    {
        foreach (GameObject rangedEnemy in rangedEnemies)
        {
            if (!rangedEnemy.activeInHierarchy && rangedEnemy.GetComponent<EnemyBehaviour>().enemyColor == EnemyBehaviour.EnemyColor.chocoBlack)
            {
                rangedEnemy.GetComponent<EnemyBehaviour>().ResetEnemy();
                return rangedEnemy;
            }
        }
        return null;
    }

    public GameObject GetFreeKamikazeWhite()
    {
        foreach (GameObject kamikazeEnemy in kamikazeEnemies)
        {
            if (!kamikazeEnemy.activeInHierarchy && kamikazeEnemy.GetComponent<EnemyBehaviour>().enemyColor == EnemyBehaviour.EnemyColor.chocoWhite)
            {
                kamikazeEnemy.GetComponent<EnemyBehaviour>().ResetEnemy();
                return kamikazeEnemy;
            }
        }
        return null;
    }

    public GameObject GetFreeKamikazeBlack()
    {
        foreach (GameObject kamikazeEnemy in kamikazeEnemies)
        {
            if (!kamikazeEnemy.activeInHierarchy && kamikazeEnemy.GetComponent<EnemyBehaviour>().enemyColor == EnemyBehaviour.EnemyColor.chocoBlack)
            {
                kamikazeEnemy.GetComponent<EnemyBehaviour>().ResetEnemy();
                return kamikazeEnemy;
            }
        }
        return null;
    }

    public GameObject GetFreeDiggerWhite()
    {
        foreach (GameObject diggerEnemy in diggerEnemies)
        {
            if (!diggerEnemy.activeInHierarchy && diggerEnemy.GetComponent<EnemyBehaviour>().enemyColor == EnemyBehaviour.EnemyColor.chocoWhite)
            {
                diggerEnemy.GetComponent<EnemyBehaviour>().ResetEnemy();
                return diggerEnemy;
            }
        }
        return null;
    }

    public GameObject GetFreeDiggerBlack()
    {
        foreach (GameObject diggerEnemy in diggerEnemies)
        {
            if (!diggerEnemy.activeInHierarchy && diggerEnemy.GetComponent<EnemyBehaviour>().enemyColor == EnemyBehaviour.EnemyColor.chocoBlack)
            {
                diggerEnemy.GetComponent<EnemyBehaviour>().ResetEnemy();
                return diggerEnemy;
            }
        }
        return null;
    }

    public GameObject GetFreeAreaEffect()
    {
        foreach (GameObject areaEffect in areaEffects)
        {
            if (!areaEffect.activeInHierarchy)
            {
                areaEffect.SetActive(true);
                return areaEffect;
            }
        }
        return null;
    }

    public GameObject GetFreeBoss()
    {
        boss.SetActive(true);
        return boss;
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
    public void SpawnPlayerBullet(Vector3 dir, Transform spawn)
    {
        GameObject bullet = GetFreePlayerBullet();
        if (bullet != null)
        {
            bullet.transform.forward = dir;
            bullet.transform.position = spawn.position;
            bullet.SetActive(true);
            bullet.GetComponent<Bullet>().type = Bullet.BulletType.PlayerBullet;
            bullet.GetComponent<Bullet>().StartBullet();
        }
    }

    public void SpawnBossBullet(Transform enemyTransform)
    {
        GameObject bullet = GetFreeBossBullet();
        if (bullet != null)
        {
            bullet.transform.position = enemyTransform.position;
            bullet.transform.rotation = enemyTransform.rotation;
            bullet.SetActive(true);
            bullet.GetComponent<Bullet>().StartBullet();
        }
    }

    public void SpawnCookieWhite(Transform enemyTransform)
    {
        GameObject cookie = GetFreeLootCookiesWhite();
        if (cookie != null)
        {
            cookie.transform.position = new Vector3(enemyTransform.position.x, enemyTransform.position.y + 0.5f, enemyTransform.position.z);
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
    public void SpawnCookieBlackOffSet(Transform enemyTransform)
    {
        GameObject cookie = GetFreeLootCookiesBlack();
        if (cookie != null)
        {
            cookie.transform.position = new Vector3(enemyTransform.position.x + 1f, enemyTransform.position.y + 0.5f, enemyTransform.position.z);
            cookie.SetActive(true);
        }
    }

    public void StartWave1()
    {
        for(int i = 0; i < totalEnemiesWave1; i++)
        {
            float angle_offset = Random.value * Mathf.PI * 2;
            float x = Mathf.Sin(angle_offset);
            float y = Mathf.Cos(angle_offset);
            Vector2 offset_direction = new Vector2(x, y);
            float new_magnitude = Random.Range(20f, 2 * 20f);
            offset_direction *= new_magnitude;
            GameObject player = FindObjectOfType<PlayerController>().gameObject;
            Vector3 newPos =  new Vector3(player.transform.position.x + offset_direction.x,0 , player.transform.position.z + offset_direction.y);

           
            if(i % 2 == 0)
            {
                GameObject basicEnemy = GetFreeBasicBlack();
                if (basicEnemy != null)
                {
                    basicEnemy.transform.position = newPos;
                    basicEnemy.SetActive(true);
                }
            }
            else
            {
                GameObject basicEnemy = GetFreeBasicWhite();
                if (basicEnemy != null)
                {
                    basicEnemy.transform.position = newPos;
                    basicEnemy.SetActive(true);
                }
            }
        }
    }

    public void StartWave2()
    {
        for (int i = 0; i < totalEnemiesWave2; i++)
        {
            float angle_offset = Random.value * Mathf.PI * 2;
            float x = Mathf.Sin(angle_offset);
            float y = Mathf.Cos(angle_offset);
            Vector2 offset_direction = new Vector2(x, y);
            float new_magnitude = Random.Range(20f, 2 * 20f);
            offset_direction *= new_magnitude;
            GameObject player = FindObjectOfType<PlayerController>().gameObject;
            Vector3 newPos = new Vector3(player.transform.position.x + offset_direction.x, 0, player.transform.position.z + offset_direction.y);

            if (i % 4 == 0)
            {
                GameObject rangedEnemy = GetFreeRangedBlack();
                if (rangedEnemy != null)
                {
                    rangedEnemy.transform.position = newPos;
                    rangedEnemy.SetActive(true);
                }
            }
            else if (i % 4 == 1)
            {
                GameObject rangedEnemy = GetFreeRangedWhite();
                if (rangedEnemy != null)
                {
                    rangedEnemy.transform.position = newPos;
                    rangedEnemy.SetActive(true);
                }
            }
            else if (i % 4 == 2)
            {
                GameObject basicEnemy = GetFreeBasicBlack();
                if (basicEnemy != null)
                {
                    basicEnemy.transform.position = newPos;
                    basicEnemy.SetActive(true);
                }
            }
            else
            {
                GameObject basicEnemy = GetFreeBasicWhite();
                if (basicEnemy != null)
                {
                    basicEnemy.transform.position = newPos;
                    basicEnemy.SetActive(true);
                }
            }
        }
    }

    public void StartWave3()
    {
        for (int i = 0; i < totalEnemiesWave3; i++)
        {
            float angle_offset = Random.value * Mathf.PI * 2;
            float x = Mathf.Sin(angle_offset);
            float y = Mathf.Cos(angle_offset);
            Vector2 offset_direction = new Vector2(x, y);
            float new_magnitude = Random.Range(20f, 2 * 20f);
            offset_direction *= new_magnitude;
            GameObject player = FindObjectOfType<PlayerController>().gameObject;
            Vector3 newPos = new Vector3(player.transform.position.x + offset_direction.x, 0, player.transform.position.z + offset_direction.y);

            if (i % 4 == 0)
            {
                GameObject rangedEnemy = GetFreeRangedBlack();
                if (rangedEnemy != null)
                {
                    rangedEnemy.transform.position = newPos;
                    rangedEnemy.SetActive(true);
                }
            }
            else if (i % 4 == 1)
            {
                GameObject rangedEnemy = GetFreeRangedWhite();
                if (rangedEnemy != null)
                {
                    rangedEnemy.transform.position = newPos;
                    rangedEnemy.SetActive(true);
                }
            }
            else if (i % 4 == 2)
            {
                GameObject kamikazeEnemy = GetFreeKamikazeBlack();
                if (kamikazeEnemy != null)
                {
                    kamikazeEnemy.transform.position = newPos;
                    kamikazeEnemy.SetActive(true);
                }
            }
            else
            {
                GameObject kamikazeEnemy = GetFreeKamikazeWhite();
                if (kamikazeEnemy != null)
                {
                    kamikazeEnemy.transform.position = newPos;
                    kamikazeEnemy.SetActive(true);
                }
            }
        }
    }

    public void StartWave4()
    {
        for(int i = 0; i < totalEnemiesWave4; i++)
        {
            float angle_offset = Random.value * Mathf.PI * 2;
            float x = Mathf.Sin(angle_offset);
            float y = Mathf.Cos(angle_offset);
            Vector2 offset_direction = new Vector2(x, y);
            float new_magnitude = Random.Range(20f, 2 * 20f);
            offset_direction *= new_magnitude;
            GameObject player = FindObjectOfType<PlayerController>().gameObject;
            Vector3 newPos = new Vector3(player.transform.position.x + offset_direction.x, 0, player.transform.position.z + offset_direction.y);

            if (i % 4 == 0)
            {
                GameObject diggerEnemy = GetFreeDiggerBlack();
                if (diggerEnemy != null)
                {
                    diggerEnemy.transform.position = newPos;
                    diggerEnemy.SetActive(true);
                }
            }
            else if (i % 4 == 1)
            {
                GameObject diggerEnemy = GetFreeDiggerWhite();
                if (diggerEnemy != null)
                {
                    diggerEnemy.transform.position = newPos;
                    diggerEnemy.SetActive(true);
                }
            }
            else if (i % 4 == 2)
            {
                GameObject kamikazeEnemy = GetFreeKamikazeBlack();
                if (kamikazeEnemy != null)
                {
                    kamikazeEnemy.transform.position = newPos;
                    kamikazeEnemy.SetActive(true);
                }
            }
            else
            {
                GameObject kamikazeEnemy = GetFreeKamikazeWhite();
                if (kamikazeEnemy != null)
                {
                    kamikazeEnemy.transform.position = newPos;
                    kamikazeEnemy.SetActive(true);
                }
            }
        }
    }

    public void StartBoss()
    {
        float angle_offset = Random.value * Mathf.PI * 2;
        float x = Mathf.Sin(angle_offset);
        float y = Mathf.Cos(angle_offset);
        Vector2 offset_direction = new Vector2(x, y);
        float new_magnitude = Random.Range(20f, 2 * 20f);
        offset_direction *= new_magnitude;
        GameObject player = FindObjectOfType<PlayerController>().gameObject;
        Vector3 newPos = new Vector3(player.transform.position.x + offset_direction.x, 0, player.transform.position.z + offset_direction.y);


        GameObject boss = GetFreeBoss();
        boss.transform.position = newPos;
        boss.SetActive(true);
    }
}
