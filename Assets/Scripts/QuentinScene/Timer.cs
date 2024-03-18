using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Timer : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerInput _playerInput;
    private InputActionAsset _inputActions;
    private InputActionMap _actionMap;

    public float startTimer = 0;
    public UnityEvent GamePaused;
    public UnityEvent GameResumed;
    public UnityEvent GameEnded;


    [Header("Component")]
    public TextMeshPro timerText;

    [Header("Timer Settings")]
    public float currentTime;

    private void Awake()
    {
        gameManager = GameManager.Instance;  
        _playerInput = FindAnyObjectByType<PlayerInput>();
        _inputActions = _playerInput.actions;
        _actionMap = _inputActions.FindActionMap("Player");
    }

    private void Start()
    {
        currentTime = startTimer;
    }

    void Update()
    {
        if(gameManager._state == GameState.IsPlaying)
        {
            currentTime += Time.deltaTime;
            timerText.text = "Timer : " + Mathf.RoundToInt(currentTime).ToString();
            FindObjectOfType<ChunkGenerate>().StartSpawnNextWave(currentTime);
            FindObjectOfType<ChunkGenerate>().StartSpawnNormalMonsters(currentTime);
        }
    }
    public void GameEnd()
    {

    }

    public void QuitGameButton()
    {

    }
}