using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storm : MonoBehaviour
{
    [SerializeField] private Transform player;
    ParticleSystem particleSystemStorm;

    private void Start()
    {
        particleSystemStorm = GetComponent<ParticleSystem>();
        player = GetComponent<Transform>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (player != null)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                particleSystemStorm.Play();
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (player != null)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                particleSystemStorm.Stop();
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Lancement tornade");
            particleSystemStorm.Play();
        }
    }
}