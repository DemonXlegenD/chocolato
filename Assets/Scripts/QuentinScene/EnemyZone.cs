using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyZone : MonoBehaviour
{
    [SerializeField] private PlayerZone playerZone;

    private bool IsEnemyInZone = false;

    private void Update()
    {
 
    }
    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si l'objet entrant est un ennemi
        if (other.CompareTag("Enemy"))
        {
            IsEnemyInZone = true;
            // Obtient le composant NavMeshAgent de l'ennemi
            Debug.Log("Enemy entre dans Zone");
            Debug.Log("Augemente vitesse");
            /*NavMeshAgent enemyAgent = other.GetComponent<NavMeshAgent>();

            if (enemyAgent != null && targetPoint != null)
            {
                // Définit la destination de l'ennemi sur le point cible
                enemyAgent.SetDestination(targetPoint.position);
            }*/
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Vérifie si l'objet entrant est un ennemi
        if (other.CompareTag("Enemy"))
        {
            // Obtient le composant NavMeshAgent de l'ennemi
            Debug.Log("Enemy encore dans Zone");
            /*NavMeshAgent enemyAgent = other.GetComponent<NavMeshAgent>();

            if (enemyAgent != null && targetPoint != null)
            {
                // Définit la destination de l'ennemi sur le point cible
                enemyAgent.SetDestination(targetPoint.position);
            }*/
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Vérifie si l'objet entrant est un ennemi
        if (other.CompareTag("Enemy"))
        {
            IsEnemyInZone = false;
            // Obtient le composant NavMeshAgent de l'ennemi
            Debug.Log("Enemy plus dans Zone");
            /*NavMeshAgent enemyAgent = other.GetComponent<NavMeshAgent>();

            if (enemyAgent != null && targetPoint != null)
            {
                // Définit la destination de l'ennemi sur le point cible
                enemyAgent.SetDestination(targetPoint.position);
            }*/
        }
    }
}
