using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    public enum EnemyType
    {
        Basic,
        Ranged,
        Kamikaze,
        Digger
    }

    public enum EnemyColor
    {
        chocoWhite,
        chocoBlack
    }


    [Header("Enemy Stats")]
    [SerializeField] EnemyType enemyType;
    [SerializeField] EnemyColor enemyColor;
    [SerializeField] float moveSpeed;
    [SerializeField] int hpMax;
    private int hpActual;
    [SerializeField] public int damage;
    [SerializeField] Slider healthBar;
    [SerializeField] float range;
    [SerializeField] float rangeContact;
    [SerializeField] float explodeTimer;
    [SerializeField] float digCooldown;
    [SerializeField] float digTimer;


    [Header("Backend")]
    [SerializeField] float timeToStartAttackingAgain;
    [SerializeField] float timeToStartMovingAgain;
    bool isMoving = true;
    bool isTouchingPlayer = false;
    bool canShoot = true;
    bool isExploding = false;
    bool isChomping = false;
    bool isUnderground = false;
    float elapsedTime = 0;
    Vector3 startDigPos;
    Vector3 endDigPos;
    Vector3 startChompPos;
    Vector3 endChompPos;
    bool isDigging = false;
    [SerializeField] PoolObjects pool;


    // Start is called before the first frame update
    void Start()
    {
        hpActual = hpMax;
        if (enemyType == EnemyType.Digger)
        {
            StartCoroutine(Dig());
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsPlayer();
        if(isExploding)
        {
            Explode();
        }
        if(isDigging)
        {
            Digging();
        }
        if(isChomping)
        {
            Chomping();
        }
    }

    void MoveTowardsPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 targetPostition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(targetPostition);
        if (player != null && isMoving)
        {
            if (EnemyType.Basic == enemyType && rangeContact < Vector3.Distance(player.transform.position, transform.position))
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), (moveSpeed * Time.fixedDeltaTime) / 5);
            }
            else if (EnemyType.Ranged == enemyType)
            {
                if (rangeContact < Vector3.Distance(player.transform.position, transform.position))
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), moveSpeed * Time.fixedDeltaTime / 5);
                }
                if (range > Vector3.Distance(player.transform.position, transform.position) && canShoot)
                {
                    StartCoroutine(Shooted());
                }
            }
            else if(EnemyType.Kamikaze == enemyType)
            {
                if(range < Vector3.Distance(player.transform.position, transform.position) && !isExploding)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), moveSpeed * Time.fixedDeltaTime / 5);
                }
                else
                {
                    isExploding = true;
                }
            }
            else if(EnemyType.Digger == enemyType)
            {
                if (rangeContact < Vector3.Distance(new Vector3(player.transform.position.x, 0, player.transform.position.z), new Vector3(transform.position.x, 0, transform.position.z)) && !isDigging)
                {
                    Debug.Log("move");
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), (moveSpeed * Time.fixedDeltaTime) / 5);
                }
                else if (isUnderground)
                {
                    if (!isChomping)
                    {
                        Debug.Log("Chomp");
                        isChomping = true;
                        gameObject.GetComponentInChildren<TrailRenderer>().enabled = false;
                        Vector3 newYPos = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
                        startChompPos = transform.position;
                        endChompPos = newYPos;
                    }
                }
            }
            if(isTouchingPlayer)
            {
                StartCoroutine(StopMoving());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isTouchingPlayer = true;
            //collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isTouchingPlayer = false;
        }
    }

    public EnemyType GetEnemyType()
    {
        return enemyType;
    }

    public EnemyColor GetEnemyColor()
    {
        return enemyColor;
    }

    public void TakeDamage(int damage)
    {
        hpActual -= damage;
        healthBar.value = hpActual;
        if (hpActual <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    
    IEnumerator StopMoving()
    {
        Debug.Log("Stop move");
        isMoving = false;
        yield return new WaitForSeconds(timeToStartMovingAgain);
        isMoving = true;
        Debug.Log("Can move Again");
    }

    IEnumerator Shooted()
    {
        canShoot = false;
        pool.SpawnEnemyBullet(transform);
        StartCoroutine(StopMoving());
        yield return new WaitForSeconds(timeToStartAttackingAgain);
        canShoot = true;
        Debug.Log("Can shoot Again");
    }

    IEnumerator Dig()
    {
        FindAnyObjectByType<AreaEffectManager>().Deactivate();
        gameObject.GetComponentInChildren<TrailRenderer>().enabled = false;
        yield return new WaitForSeconds(digCooldown);
        isDigging = true;
        startDigPos = transform.position;
        endDigPos = new Vector3(startDigPos.x, startDigPos.y - 1.5f, startDigPos.z);
    }

    void Digging()
    {
        if (elapsedTime < digTimer)
        {
            transform.position = Vector3.Lerp(startDigPos, endDigPos, elapsedTime / digTimer);
            elapsedTime += Time.deltaTime;
        }
        else if(elapsedTime >= digTimer)
        {
            isDigging = false;
            isUnderground = true;
            elapsedTime = 0;
            gameObject.GetComponentInChildren<TrailRenderer>().enabled = true;
        }
    }

    void Explode()
    {
        AreaEffectManager areaEffect = FindAnyObjectByType<AreaEffectManager>();
        bool exploded = areaEffect.Activate(enemyType, transform.position, new Vector3(6, startDigPos.y, 6), explodeTimer);
        if (exploded)
        {
            areaEffect.gameObject.transform.GetChild(0).gameObject.GetComponent<AreaExplosion>().Explode(damage);
        }
    }

    void Chomping()
    {
        AreaEffectManager areaEffect = FindAnyObjectByType<AreaEffectManager>();
        bool chomped = areaEffect.Activate(enemyType, transform.position, new Vector3(6, startDigPos.y, 6), 1f);
        Debug.Log("Chomping");
        if(!chomped)
        {
            transform.position = Vector3.Lerp(startChompPos, endChompPos, areaEffect.elapsedTime / 1f);
        }
        else
        {
            areaEffect.gameObject.transform.GetChild(0).gameObject.GetComponent<AreaExplosion>().Explode(damage);
            isChomping = false;
            isUnderground = false;
            StartCoroutine(Dig());
        }
    }
}
