using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEffectManager : MonoBehaviour
{
    GameObject explosionTrigger;
    GameObject areaEffect;
    Vector3 startScale = new Vector3(1f, 0.1f, 1f);
    public float elapsedTime = 0;
    bool activated = false;
    // Start is called before the first frame update
    void Start()
    {
        explosionTrigger = transform.GetChild(0).gameObject;
        areaEffect = transform.GetChild(1).gameObject;
        explosionTrigger.SetActive(false);
        areaEffect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Activate(GameObject enemy, Vector3 newPos, Vector3 endScale, float timer)
    {
        if (!activated)
        {
            activated = true;
            explosionTrigger.SetActive(true);
            areaEffect.SetActive(true);
            transform.position = new Vector3(newPos.x, 0, newPos.z);
            GetComponentInChildren<AreaExplosion>().enemy = enemy;
        }
        return loopTriggerEnlargement(endScale, timer);
    }

    bool loopTriggerEnlargement(Vector3 endScale,float timer)
    {
        if (elapsedTime < timer)
        {

            transform.GetChild(0).gameObject.transform.localScale = Vector3.Lerp(startScale, new Vector3(6, endScale.y, 6), elapsedTime / timer);
            elapsedTime += Time.deltaTime;
            return false;
        }
        else
        {
            elapsedTime = 0;
            activated = false;
            return true;
        }

    }

    public void Deactivate()
    {
        explosionTrigger.SetActive(false);
        areaEffect.SetActive(false);
        elapsedTime = 0;
        explosionTrigger.transform.localScale = startScale;
        activated = false;
    }
}
