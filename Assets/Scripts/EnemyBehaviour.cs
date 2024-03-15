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
    [SerializeField] public EnemyType enemyType;
    [SerializeField] public EnemyColor enemyColor;
    [SerializeField] float moveSpeed;
    [SerializeField] int hpMax;
    private int hpActual;
    [SerializeField] public int damage;
    [SerializeField] Slider healthBar;
    [SerializeField] float range;
    [SerializeField] float rangeContact;
    [SerializeField] float explodeTimer;
    [SerializeField] float digTimer;


    [Header("Backend")]
    [SerializeField] float timeToStartAttackingAgain;
    [SerializeField] float timeToStartMovingAgain;
    bool isMoving = true;
    bool isTouchingPlayer = false;
    bool canShoot = true;
    bool isExploding = false;
    float elapsedTime = 0;
    Vector3 startScaleExplosionArea = new Vector3(1f, 0.1f, 1f);
    Vector3 startDigPos;
    bool isDigging = false;
    [SerializeField] PoolObjects pool;


    // Start is called before the first frame update
    void Start()
    {
        hpActual = hpMax;
        /*transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);*/
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyType == EnemyType.Digger)
        {
            StartCoroutine(Dig());
        }
        MoveTowardsPlayer();
        if (isExploding)
        {
            Explode();
        }
        if (isDigging)
        {
            startDigPos = transform.position;
            Digging();
        }
    }

    void MoveTowardsPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        transform.LookAt(player.transform);
        if (player != null && isMoving)
        {
            if (EnemyType.Basic == enemyType && rangeContact < Vector3.Distance(player.transform.position, transform.position))
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, (moveSpeed * Time.fixedDeltaTime) / 5);
            }
            else if (EnemyType.Ranged == enemyType)
            {
                if (rangeContact < Vector3.Distance(player.transform.position, transform.position))
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.fixedDeltaTime / 5);
                }
                if (range > Vector3.Distance(player.transform.position, transform.position) && canShoot)
                {
                    StartCoroutine(Shooted());
                }
            }
            else if (EnemyType.Kamikaze == enemyType)
            {
                if (range < Vector3.Distance(player.transform.position, transform.position) && !isExploding)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.fixedDeltaTime / 5);
                }
                else
                {
                    isExploding = true;
                    transform.GetChild(0).gameObject.SetActive(true);
                    transform.GetChild(1).gameObject.SetActive(true);
                }
            }
            else if (EnemyType.Digger == enemyType)
            {
                if (range < Vector3.Distance(player.transform.position, transform.position) && !isDigging)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, (moveSpeed * Time.fixedDeltaTime) / 5);
                }
            }
            if (isTouchingPlayer)
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
        pool.SpawnBullet(transform);
        StartCoroutine(StopMoving());
        yield return new WaitForSeconds(timeToStartAttackingAgain);
        canShoot = true;
        Debug.Log("Can shoot Again");
    }

    IEnumerator Dig()
    {
        yield return new WaitForSeconds(digTimer);
        isDigging = true;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);

        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(1).transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
    }

    void Digging()
    {
        if (elapsedTime < digTimer)
        {
            transform.GetChild(0).gameObject.transform.localScale = Vector3.Lerp(startDigPos, new Vector3(startDigPos.x, startDigPos.y - 1, startDigPos.z), elapsedTime / explodeTimer);
            elapsedTime += Time.deltaTime;
        }
    }

    void Explode()
    {
        if (elapsedTime < explodeTimer)
        {
            transform.GetChild(0).gameObject.transform.localScale = Vector3.Lerp(startScaleExplosionArea, new Vector3(6, startScaleExplosionArea.y, 6), elapsedTime / explodeTimer);
            elapsedTime += Time.deltaTime;
        }
        if (elapsedTime >= explodeTimer)
        {
            AreaExplosion childAreaExplosion = transform.GetChild(0).gameObject.GetComponent<AreaExplosion>();
            childAreaExplosion.Explode();
            elapsedTime = 0;
        }
    }

    public void Death()
    {
        EventManager.instance.EnemyDeath(enemyColor);
    }
}
