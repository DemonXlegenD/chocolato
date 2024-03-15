using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletType { EnemyBullet, PlayerBullet }
    public BulletType type;

    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletDamage;
    // Start is called before the first frame update
    void Start()
    {
    }

    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(8f);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (type == BulletType.EnemyBullet)
        {
            if (other.gameObject.tag == "Player")
            {
                gameObject.SetActive(false);
                //collision.gameObject.GetComponent<PlayerController>().TakeDamage(bulletDamage);
            }
        }
        else
        {
            if (other.gameObject.tag == "Enemy")
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void StartBullet()
    {
        StartCoroutine(DestroyBullet());
        if (type == BulletType.EnemyBullet)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            transform.forward = player.transform.position - transform.position;
        }
    }
}