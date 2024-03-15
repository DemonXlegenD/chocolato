using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorMap : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Obstacle obstacle1;
    [SerializeField] private Obstacle obstacle2;
    void Start()
    {
        obstacle1.Generate();
        obstacle2.Generate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
