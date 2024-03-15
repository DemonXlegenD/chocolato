using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    public EnemyDeathEvent onEnemyDeath = new EnemyDeathEvent();

    public static EventManager GetInstance()
    {
        if (instance == null)
            return instance = FindAnyObjectByType<EventManager>();
        else 
            return instance;
    }

    private void Awake()
    {
        GetInstance();
    }

    public void EnemyDeath(EnemyBehaviour.EnemyColor color)
    {
        onEnemyDeath.Invoke(color);
    }
}

[System.Serializable]
public class EnemyDeathEvent : UnityEvent<EnemyBehaviour.EnemyColor>
{
}
