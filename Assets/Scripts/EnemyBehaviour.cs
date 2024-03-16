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
        Digger,
        Boss 
    }

    public enum EnemyColor
    {
        chocoWhite,
        chocoBlack,
        any
    }


    [Header("Enemy Stats")]
    [SerializeField] public EnemyType enemyType;
    [SerializeField] public EnemyColor enemyColor;
    [SerializeField] float moveSpeed;
    [SerializeField] int hpMax;
    private float hpActual;
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
    [SerializeField] PoolObjects pool;
    AreaEffectManager areaEffectManager;
    bool isRunning = false;
    bool phase2Activated = false;
    bool phase3Activated = false;

    [Header("Animation")]
    //[SerializeField] string nameAnime;
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
    Animator animator;
    ParticleSystem part;
    int isDeadHash = Animator.StringToHash("IsDead");

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        pool = FindAnyObjectByType<PoolObjects>();
    }
    // Start is called before the first frame update
    void Start()
    {
        hpActual = hpMax;
        if (enemyType == EnemyType.Digger)
        {
            StartCoroutine(Dig());
        }
        else
        {
            gameObject.GetComponentInChildren<TrailRenderer>().enabled = false;
        }
        part = GetComponentInChildren<ParticleSystem>();
        //La fonction test c'est pour les particule quand il est mort il faudrat le link 
        // le contenu dans la fonction dead
        //StartCoroutine(Test());
        //Quand on va link l'asset au prefab faudrat mettre le nom de l'asset mais si on le met pas tout de suite commenter le
        //animator.SetBool($"{nameAnime}", true);

    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsPlayer();
        if (isExploding)
        {
            Explode();
        }
        if (isDigging)
        {
            Digging();
        }
        if (isChomping)
        {
            Chomping();
        }
        if(enemyType == EnemyType.Boss)
        {
            int tmp = hpMax / 4;
            if(hpActual <= tmp*2 && !phase2Activated)
            {
                phase2Activated = true;
                moveSpeed *= 2f;
            }
            if(hpActual <= tmp*1 && !phase3Activated)
            {
                phase3Activated = true;
            }
            if(phase3Activated && canShoot)
            {
                StartCoroutine(ShootedBoss());
            }
        }
        if(hpActual <= 0)
        {
            if (areaEffectManager != null)
            {
                areaEffectManager.Deactivate();
                areaEffectManager = null;
            }
        }
    }

    void MoveTowardsPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(targetPosition);
        if (player != null && isMoving)
        {
            if (EnemyType.Basic == enemyType && rangeContact < Vector3.Distance(player.transform.position, transform.position))
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, (moveSpeed * Time.fixedDeltaTime) / 5);
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
            else if (EnemyType.Kamikaze == enemyType)
            {
                if (range < Vector3.Distance(player.transform.position, transform.position) && !isExploding)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), moveSpeed * Time.fixedDeltaTime / 5);
                }
                else
                {
                    isExploding = true;
                }
            }
            else if (EnemyType.Digger == enemyType)
            {
                if (rangeContact < Vector3.Distance(new Vector3(player.transform.position.x, 0, player.transform.position.z), new Vector3(transform.position.x, 0, transform.position.z)) && !isDigging)
                {
                    //Debug.Log("move");
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), (moveSpeed * Time.fixedDeltaTime) / 5);
                }
                else if (isUnderground)
                {
                    if (!isChomping)
                    {
                        //Debug.Log("Chomp");
                        isChomping = true;
                        gameObject.GetComponentInChildren<TrailRenderer>().enabled = false;
                        Vector3 newYPos = new Vector3(transform.position.x, transform.position.y + 1.7f, transform.position.z);
                        startChompPos = transform.position;
                        endChompPos = newYPos;
                    }
                }
            }
            else if (EnemyType.Boss == enemyType)
            {
                if (rangeContact < Vector3.Distance(new Vector3(player.transform.position.x, 0, player.transform.position.z), new Vector3(transform.position.x, 0, transform.position.z)))
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), (moveSpeed * Time.fixedDeltaTime) / 5);
                }
            }
            if (isTouchingPlayer)
            {
                StartCoroutine(StopMoving());
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isTouchingPlayer = true;
            collision.gameObject.GetComponent<PlayerController>().GetDamaged(damage);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
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

    public void TakeDamage(float damage)
    {
        hpActual -= damage;
        //healthBar.value = hpActual;
        if (hpActual <= 0)
        {
            gameObject.GetComponentInChildren<ParticleSystem>().Play();
            isExploding = false;
            animator.SetBool(isDeadHash, true);
            Death();
            //gameObject.SetActive(false);
            if(enemyColor == EnemyColor.chocoWhite)
            {
                pool.SpawnCookieWhite(transform);
            }
            else if (enemyColor == EnemyColor.chocoBlack)
            {
                pool.SpawnCookieBlack(transform);
            }
            else if (enemyColor == EnemyColor.any)
            {
                Transform tmp = transform;
                tmp.position = new Vector3(tmp.position.x , 0, tmp.position.z);
                pool.SpawnCookieBlackOffSet(tmp);
                pool.SpawnCookieWhite(tmp);
            }
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

    IEnumerator ShootedBoss()
    {
        canShoot = false;
        pool.SpawnBossBullet(transform);
        StartCoroutine(StopMoving());
        yield return new WaitForSeconds(timeToStartAttackingAgain);
        canShoot = true;
        //Debug.Log("Can shoot Again");
    }
    IEnumerator Dig()
    {
        if (areaEffectManager != null)
        {
            areaEffectManager.Deactivate();
            areaEffectManager = null;
        }
        gameObject.GetComponentInChildren<TrailRenderer>().enabled = false;
        yield return new WaitForSeconds(digCooldown);
        isDigging = true;
        startDigPos = transform.position;
        GetComponent<Rigidbody>().isKinematic = true;
        endDigPos = new Vector3(startDigPos.x, startDigPos.y - 1.7f, startDigPos.z);
    }

    void Digging()
    {
        if (elapsedTime < digTimer)
        {
            transform.position = Vector3.Lerp(startDigPos, endDigPos, elapsedTime / digTimer);
            elapsedTime += Time.deltaTime;
        }
        else if (elapsedTime >= digTimer)
        {
            isDigging = false;
            isUnderground = true;
            elapsedTime = 0;
            gameObject.GetComponentInChildren<TrailRenderer>().enabled = true;
        }
    }

    public float GetHp()
    {
        return hpActual;
    }

    void Explode()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        if(areaEffectManager == null)
        {
            areaEffectManager = pool.GetFreeAreaEffect().GetComponent<AreaEffectManager>();
        }
        else
        {
            bool exploded = areaEffectManager.Activate(gameObject, new Vector3(transform.position.x,0.3f,transform.position.z), new Vector3(6, 0.01f, 6), explodeTimer);
            if (exploded)
            {
                areaEffectManager.gameObject.transform.GetChild(0).gameObject.GetComponent<AreaExplosion>().Explode(damage);
            }
        }
    }

    void Chomping()
    {
        if (areaEffectManager == null)
        {
            areaEffectManager = pool.GetFreeAreaEffect().GetComponent<AreaEffectManager>();
        }
        else
        {
            bool chomped = areaEffectManager.Activate(gameObject, new Vector3(transform.position.x, 0.3f, transform.position.z), new Vector3(6, 0.01f, 6), 1f);
            if (!chomped)
            {
                transform.position = Vector3.Lerp(startChompPos, endChompPos, areaEffectManager.elapsedTime / 1f);
            }
            else
            {
                areaEffectManager.gameObject.transform.GetChild(0).gameObject.GetComponent<AreaExplosion>().Explode(damage);
                isChomping = false;
                isUnderground = false;
                GetComponent<Rigidbody>().isKinematic = false;
                StartCoroutine(Dig());
            }
        }
    }

    IEnumerator Test()
    {
        part.Play();
        yield return new WaitForSeconds(2f);
        part.Stop();
    }
    public void Death()
    {
        EventManager.GetInstance().EnemyDeath(enemyColor);
    }
}
