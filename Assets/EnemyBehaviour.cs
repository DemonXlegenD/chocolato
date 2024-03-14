using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    enum EnemyType
    {
        Basic,
        Ranged,
        Kamikaze,
        Mole
    }

    enum EnemyColor
    {
        chocoWhite,
        chocoBlack
    }


    [Header("Enemy Stats")]
    [SerializeField] EnemyType enemyType;
    [SerializeField] float moveSpeed;
    [SerializeField] int hpMax;
    private int hpActual;
    [SerializeField] public int damage;
    [SerializeField] Slider healthBar;
    [SerializeField] float range;
    [SerializeField] float rangeContact;
    [SerializeField] float explodeTimer;


    [Header("Backend")]
    [SerializeField] float timeToStartAttackingAgain;
    [SerializeField] float timeToStartMovingAgain;
    bool isMoving = true;
    bool isTouchingPlayer = false;
    bool canShoot = true;
    bool isExploding = false;
    float elapsedTime = 0;
    Vector3 startScaleExplosionArea = new Vector3(1f, 0.1f, 1f);
    [SerializeField] PoolObjects pool;


    // Start is called before the first frame update
    void Start()
    {
        hpActual = hpMax;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsPlayer();
        if(isExploding)
        {
            Explode();
        }
    }

    void MoveTowardsPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
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
                if (range < Vector3.Distance(player.transform.position, transform.position) && canShoot)
                {
                    StartCoroutine(Shooted());
                }
            }
            else if(EnemyType.Kamikaze == enemyType)
            {
                if(range < Vector3.Distance(player.transform.position, transform.position) && !isExploding)
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
        GameObject bullet = pool.GetFreeBullet();
        bullet.transform.position = transform.position;
        bullet.transform.rotation = transform.rotation;
        bullet.SetActive(true);
        bullet.GetComponent<Bullet>().StartBullet();
        StartCoroutine(StopMoving());
        yield return new WaitForSeconds(timeToStartAttackingAgain);
        canShoot = true;
        Debug.Log("Can shoot Again");
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
        }
    }
}
