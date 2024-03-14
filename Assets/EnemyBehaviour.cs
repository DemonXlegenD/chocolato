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
    [SerializeField] int damage;
    [SerializeField] Slider healthBar;
    [SerializeField] float range;
    [SerializeField] float rangeContact;

    [Header("Backend")]
    [SerializeField] float timeToStartAttackingAgain;
    [SerializeField] float timeToStartMovingAgain;
    bool isMoving = true;
    [SerializeField] GameObject bulletPrefab;
    bool isTouchingPlayer = false;
    bool canShoot = true;


    // Start is called before the first frame update
    void Start()
    {
        hpActual = hpMax;
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && isMoving)
        {
            if (EnemyType.Basic == enemyType && range < Vector3.Distance(player.transform.position, transform.position))
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
        Instantiate(bulletPrefab, transform.position, transform.rotation);
        StartCoroutine(StopMoving());
        yield return new WaitForSeconds(timeToStartAttackingAgain);
        canShoot = true;
        Debug.Log("Can shoot Again");
    }

}
