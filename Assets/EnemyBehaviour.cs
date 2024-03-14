using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] int moveSpeed;
    [SerializeField] int hpMax;
    private int hpActual;
    [SerializeField] int damage;
    [SerializeField] Slider healthBar;

    [Header("backend")]
    [SerializeField] float timeToStartAttackAgain;
    bool isAttacking = true;

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
        if (player != null && isAttacking)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, (moveSpeed * Time.fixedDeltaTime)/5);
        }
    }

    private void OnCollisionEnter (Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            StartCoroutine(Attacked());
        }
    }

    IEnumerator Attacked()
    {
        isAttacking = false;
        yield return new WaitForSeconds(timeToStartAttackAgain);
        isAttacking = true;
        Debug.Log("Attacking Again");
    }
}