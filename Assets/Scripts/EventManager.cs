using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    public UnityEvent onEnemyDeath = new UnityEvent();

    void SetInstance()
    {
        if (instance == null)
            instance = this;
    }

    private void Awake()
    {
        SetInstance();
    }
    public void EnemyDeath()
    {
        onEnemyDeath.Invoke();
    }
}
