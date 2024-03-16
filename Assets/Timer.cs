using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Timer : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputActionAsset _inputActions;
    private InputActionMap _actionMap;

    public float startTimer = 0;
    public UnityEvent GamePaused;
    public UnityEvent GameResumed;
    private bool _isPaused;

    [Header("Component")]
    public TextMeshProUGUI timerText;

    [Header("Timer Settings")]
    public float currentTime;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _inputActions = _playerInput.actions;
        _actionMap = _inputActions.FindActionMap("Player");
    }

    private void Start()
    {
        currentTime = startTimer;
    }

    void Update()
    {
        currentTime = currentTime += Time.deltaTime;
        timerText.text = currentTime.ToString();
        timerText.text = currentTime.ToString("0.0");


        if (_actionMap.FindAction("Pause").WasPerformedThisFrame())
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