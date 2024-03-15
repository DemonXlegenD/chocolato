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
    float elapsedTime = 0;
    Vector3 startScaleExplosionArea = new Vector3(1f, 0.1f, 1f);
    Vector3 startDigPos;
    Vector3 endDigPos;
    bool isDigging = false;
    [SerializeField] PoolObjects pool;


    // Start is called before the first frame update
    void Start()
    {
        hpActual = hpMax;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        if (enemyType == EnemyType.Digger)
        {
            StartCoroutine(Dig());
            transform.GetChild(0).transform.position = new Vector3(transform.GetChild(0).transform.position.x, transform.GetChild(0).transform.position.y+1.5f, transform.GetChild(0).transform.position.z);
            transform.GetChild(1).transform.position = new Vector3(transform.GetChild(1).transform.position.x, transform.GetChild(1).transform.position.y + 1.5f, transform.GetChild(1).transform.position.z); ;
            transform.GetChild(2).transform.position = new Vector3(transform.GetChild(2).transform.position.x, transform.GetChild(2).transform.position.y + 1.5f, transform.GetChild(2).transform.position.z); ;
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
                    transform.GetChild(0).gameObject.SetActive(true);
                    transform.GetChild(1).gameObject.SetActive(true);
                }
            }
            else if(EnemyType.Digger == enemyType)
            {
                if (rangeContact < Vector3.Distance(new Vector3(player.transform.position.x, 0, player.transform.position.z), new Vector3(transform.position.x, 0, transform.position.z)) && !isDigging)
                {
                    Debug.Log("move");
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), (moveSpeed * Time.fixedDeltaTime) / 5);
                    if (!isChomping)
                    {
                        Debug.Log("Chomp");
                        isChomping = true;
                        transform.GetChild(0).gameObject.SetActive(true);
                        transform.GetChild(1).gameObject.SetActive(true);
                        gameObject.GetComponentInChildren<TrailRenderer>().enabled = false;
                        Vector3 newYPos = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
                        startDigPos = transform.position;
                        endDigPos = newYPos;
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
        //Debug.Log("Stop move");
        isMoving = false;
        yield return new WaitForSeconds(timeToStartMovingAgain);
        isMoving = true;
        //Debug.Log("Can move Again");
    }

    IEnumerator Shooted()
    {
        canShoot = false;
        pool.SpawnEnemyBullet(transform);
        StartCoroutine(StopMoving());
        yield return new WaitForSeconds(timeToStartAttackingAgain);
        canShoot = true;
        //Debug.Log("Can shoot Again");
    }

    IEnumerator Dig()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(0).gameObject.transform.localScale = startScaleExplosionArea;
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
            elapsedTime = 0;
            gameObject.GetComponentInChildren<TrailRenderer>().enabled = true;
        }
    }

    void Explode()
    {
        if(elapsedTime < explodeTimer)
        {
            transform.GetChild(0).gameObject.transform.localScale = Vector3.Lerp(startScaleExplosionArea, new Vector3(6, startScaleExplosionArea.y, 6), elapsedTime / explodeTimer);
            elapsedTime += Time.deltaTime;
        }
        if(elapsedTime >= explodeTimer)
        {
            AreaExplosion childAreaExplosion = transform.GetChild(0).gameObject.GetComponent<AreaExplosion>();
            childAreaExplosion.Explode();
            elapsedTime = 0;
        }
    }

    void Chomping()
    {
        Debug.Log("Chomping");
        if (elapsedTime < 1f)
        {
            transform.GetChild(0).gameObject.transform.localScale = Vector3.Lerp(startScaleExplosionArea, new Vector3(6, startScaleExplosionArea.y, 6), elapsedTime / 1f);
            transform.position = Vector3.Lerp(startDigPos, endDigPos, elapsedTime / 1f);
            elapsedTime += Time.deltaTime;
        }
        else if (elapsedTime >= 1f)
        {
            isChomping = false;
            elapsedTime = 0;
            StartCoroutine(Dig());
        }
    }
}
