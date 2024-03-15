using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerZone : MonoBehaviour
{

    private bool IsPlayerInZone = false;
    private bool IsZoneCaptured = false;
    private bool IsRecup = false;
    UnityEvent OnCapture;

    [SerializeField] private float delayArea = 10;
    [SerializeField] private float delayMax = 10;

    private void Update()
    {
        if (IsPlayerInZone)
        {
            delayArea -= Time.deltaTime;
        }

        if (delayArea != delayMax)
        {
            if (IsRecup)
            {
                if (delayArea >= delayMax)
                {
                    delayArea = delayMax;
                }
                else
                {
                    delayArea += Time.deltaTime / 2f;
                }
            }
            else
            {
                StartRecup(2f);
            }
        }


        if (delayArea <= 0)
        {
            IsZoneCaptured = true;
            OnCapture?.Invoke();
            Debug.Log("Lancer l'événement météorité puis à l'impact supprimer la zone");
        }
    }

    private IEnumerator StartRecup(float delay)
    {
        yield return new WaitForSeconds(delay);

        IsRecup = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsPlayerInZone = true;
            Debug.Log("Player inside");
        }
    }

    private void OnTriggerStay(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsPlayerInZone = false;
            Debug.Log("Player outside");
        }
    }

    #region Getter

    public bool GetPlayerInZone() { return IsPlayerInZone; }
    #endregion
}
