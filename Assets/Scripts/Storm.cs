using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storm : MonoBehaviour
{
    [SerializeField] float strength = 0.075f;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // TakeDamage()
            Debug.Log("sa touche");
            other.transform.Translate(new Vector3(other.transform.position.x - transform.position.x,0, other.transform.position.z - transform.position.z).normalized * strength);
        }
    }
}