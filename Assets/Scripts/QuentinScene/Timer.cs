using JetBrains.Annotations;
using Nova.TMP;
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
    private TextMeshProTextBlock timerText;

    [Header("Timer Settings")]
    public float currentTime;

    private void Awake()
    {
        timerText = GetComponent<TextMeshProTextBlock>();
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
            string timer = "Timer : " + Mathf.RoundToInt(currentTime).ToString();
            timerText.text = timer;
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