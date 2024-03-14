using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] private float timerCounter;
    [SerializeField] private float minutes;
    [SerializeField] private float secondes;

   
    void Update()
    {
        timerCounter += Time.deltaTime;
        minutes = Mathf.FloorToInt(timerCounter / 60f); 
        secondes = Mathf.FloorToInt(timerCounter - (minutes / 60f));
        timeText.text = string.Format("{0:00}:{1:00}", minutes, secondes);
    }
}
