using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Loot : MonoBehaviour
{
    [SerializeField] EnemyBehaviour.EnemyColor color;
    AudioSource son;
    [SerializeField] AudioClip lootSound;
    // Start is called before the first frame update
    void Start()
    {
        son = GetComponent<AudioSource>();
        son.clip = lootSound;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {   
            Debug.Log("Loot");
            son.Play(0);
            FindObjectOfType<PlayerController>().AddExp(color);
            GetComponent<MeshRenderer>().enabled = false;
            StartCoroutine(Deactivate());
            
        }
    }

    IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(1);
        transform.parent.gameObject.SetActive(false);
        GetComponent<MeshRenderer>().enabled = true;
    }

}
