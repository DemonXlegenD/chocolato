using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletDamage;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyBullet());
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        transform.forward = player.transform.position - transform.position;
    }

    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(8f);
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            //collision.gameObject.GetComponent<PlayerController>().TakeDamage(bulletDamage);
        }
    }
}