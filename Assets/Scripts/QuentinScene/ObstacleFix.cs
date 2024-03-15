using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleFix : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.GetChild(0).transform.position.y < -20f)
        {
            Destroy(gameObject);
        }
    }
}
