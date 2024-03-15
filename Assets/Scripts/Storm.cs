using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storm : MonoBehaviour
{
    [SerializeField] private GameObject player;
    Rigidbody rb;

    private void Start()
    {
        rb = player.GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (player != null)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                // TakeDaMAGE()
                Debug.Log("sa touche");
                rb.AddForce(3 * Time.deltaTime * Vector3.back);
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (player != null)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                // ne plus faire de dégats en continue
            }
        }
    }
}