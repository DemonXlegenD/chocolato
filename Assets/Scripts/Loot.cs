using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Loot : MonoBehaviour
{
    [SerializeField] EnemyBehaviour.EnemyColor color;
    AudioSource son;
    // Start is called before the first frame update
    void Start()
    {
        son = GetComponent<AudioSource>();
        son.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {   
            StartCoroutine(playSon());
            FindObjectOfType<PlayerController>().AddExp(color);
            gameObject.SetActive(false);
            
        }
    }

    private IEnumerator playSon()
    {
        son.Play();
        yield return new WaitForSeconds(3);
        son.Stop();
    }
}
