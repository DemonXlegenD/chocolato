using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoockiesRotation : MonoBehaviour
{

    public float rotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.Euler(0, 0, rotation);
    }
}
