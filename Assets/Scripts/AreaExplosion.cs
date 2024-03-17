using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaExplosion : MonoBehaviour
{

    private bool explode = false;
    public GameObject enemy;
    private List<Collider> playersInArea = new List<Collider>();

    int damage; 

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (explode && playersInArea.Count == 0)
        {
            if(enemy.GetComponent<EnemyBehaviour>().GetEnemyType() == EnemyBehaviour.EnemyType.Kamikaze)
            {
                Deactivate();
            }
            else if(enemy.GetComponent<EnemyBehaviour>().GetEnemyType() == EnemyBehaviour.EnemyType.Digger)
            {
                Chomped();
            }
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
            other.GetComponent<PlayerController>().GetDamaged(damage);
            explode = false;
            if (enemy.GetComponent<EnemyBehaviour>().GetEnemyType() == EnemyBehaviour.EnemyType.Kamikaze)
            {
                Deactivate();
            }
            else if (enemy.GetComponent<EnemyBehaviour>().GetEnemyType() == EnemyBehaviour.EnemyType.Digger)
            {
                Chomped();
            }
        }
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
        transform.parent.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        enemy.SetActive(false);
        explode = false;
    }

    private void Chomped()
    {
        gameObject.SetActive(false);
        transform.parent.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        explode = false;
    }

    public void Explode(int _damage)
    {
        damage = _damage;
        explode = true;
    }
}
