using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaExplosion : MonoBehaviour
{

    private bool explode = false;
    private List<Collider> playersInArea = new List<Collider>();

    int damage; 

    // Start is called before the first frame update
    void Start()
    {
        damage = GetComponentInParent<EnemyBehaviour>().damage;
    }

    // Update is called once per frame
    void Update()
    {
        if (explode && playersInArea.Count == 0)
        {
           Deactivate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playersInArea.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playersInArea.Remove(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && explode)
        {
            //other.GetComponent<PlayerController>().TakeDamage(damage);
            explode = false;
            Debug.Log("player hit by explosion");
            Deactivate();
        }
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
        transform.parent.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
        explode = false;
    }
    public void Explode()
    {
        Debug.Log("explosion");
        explode = true;
    }
}
