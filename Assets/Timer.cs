using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [Header("Component")]
    public TextMeshProUGUI timerText;

    [Header("Timer Settings")]
    public float currentTime;

    void Update()
    {
        currentTime = currentTime += Time.deltaTime;
        timerText.text = currentTime.ToString();
        timerText.text = currentTime.ToString("0.0");
    }
}
