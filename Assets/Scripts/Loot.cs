using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Loot : MonoBehaviour
{
    [SerializeField] EnemyBehaviour.EnemyColor color;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            FindObjectOfType<PlayerController>().AddExp(color);
            gameObject.SetActive(false);
        }
    }
}
