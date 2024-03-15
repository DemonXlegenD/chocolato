using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseControler : MonoBehaviour
{
    public UnityEvent GamePaused;
    public UnityEvent GameResumed;
    private bool _isPaused;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _isPaused = !_isPaused;
        }
        if (_isPaused)
        {
            Time.timeScale = 0;
            GamePaused.Invoke();
        }
        else
        {
            Time.timeScale = 1;
            GameResumed.Invoke();
        }
    }
}
