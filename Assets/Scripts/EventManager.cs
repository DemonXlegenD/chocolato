using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    public EnemyDeathEvent onEnemyDeath = new EnemyDeathEvent();

    void SetInstance()
    {
        if (instance == null)
            instance = this;
    }

    private void Awake()
    {
        SetInstance();
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
