using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.DefaultInputActions;

public class PlayerController : MonoBehaviour
{
    enum PlayerState { normal, dashing, hitted };
    enum ChocoState { chocoWhite, chocoBlack, chocoMilk };

    [Header("Player Input")]
    private PlayerInput _playerInput;
    private InputActionAsset _inputActions;
    private InputActionMap _actionMap;

    [Header("Player Stats")]
    [SerializeField] int life;
    [SerializeField] int speed;
    [SerializeField] int baseSpeed;
    [SerializeField] int damage;
    [SerializeField] float dashTimer;
    [SerializeField] float dashBaseTime;
    [SerializeField] int dashSpeed;
    [SerializeField] Vector2 lastDirection;
    [SerializeField] float colorState;
    [SerializeField] float colorEvolve;
    [SerializeField] float colorTimer;
    [SerializeField] float colorTick;
    [SerializeField] float attackTimer;
    [SerializeField] float attackTick;


    [Header("State")]
    [SerializeField] PlayerState playerState;
    [SerializeField] ChocoState chocoState;

    [Header("Components")]
    [SerializeField] Rigidbody rb;
    [SerializeField] Slider colorSlider;
    [SerializeField] GameObject playerBody;
    [SerializeField] Animator animator;

    [Header("Shoot")]
    [SerializeField] GameObject shootPoint;
    [SerializeField] GameObject prefabBullet;
    [SerializeField] PoolObjects pool;


    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        shootPoint = GameObject.FindWithTag("ShootPoint");
        _playerInput = GetComponent<PlayerInput>();
        _inputActions = _playerInput.actions;
        _actionMap = _inputActions.FindActionMap("Player");
        colorState = colorSlider.value / 2;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerState == PlayerState.normal || playerState == PlayerState.hitted)
        {
            Rotate();
            if (_actionMap.FindAction("Dash").WasPressedThisFrame())
            {
                Dash();
            }
        }
        TickTimers();
        if (_actionMap.FindAction("SwapColor").WasPressedThisFrame())
        {
            if (chocoState == ChocoState.chocoWhite)
            {
                chocoState = ChocoState.chocoBlack;
                colorState -= colorEvolve;
            }
            else
            {
                chocoState = ChocoState.chocoWhite;
                colorState += colorEvolve;
            }
            colorTimer = colorTick;
            attackTimer = attackTick;
        }
        SetColor();
    }
    void FixedUpdate()
    {
        if (playerState == PlayerState.normal || playerState == PlayerState.hitted)
        {
            Move();
        }
    }

    private void Move()
    {
        Vector3 moveInput = _actionMap.FindAction("Move").ReadValue<Vector2>();
        moveInput.Normalize();
        if (moveInput != Vector3.zero)
        {
            lastDirection = moveInput;
            rb.velocity = new Vector3(moveInput.x, 0, moveInput.y) * speed;
            Debug.Log("Moving");
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    void Rotate()
    {
        Vector3 rotateInput = _actionMap.FindAction("Rotate").ReadValue<Vector2>();
        rotateInput.Normalize();
        if (rotateInput.magnitude > 0.25)
        {
            playerBody.transform.forward = new Vector3(rotateInput.x, 0, rotateInput.y);
        }
    }

    void Dash()
    {
        dashTimer = dashBaseTime;
        playerState = PlayerState.dashing;
    }
    void SetColor()
    {
        if (colorTimer <= 0 && colorState > 0 && colorState < colorSlider.maxValue)
        {
            colorTimer = colorTick;
            if (chocoState == ChocoState.chocoWhite)
            {
                colorState += colorEvolve;
            }
            else
            {
                colorState -= colorEvolve;
            }
        }
        else if (colorTimer > 0)
        {
            colorTimer -= Time.deltaTime;
        }
        colorSlider.value = colorState;
    }
    void Attack()
    {
        if (attackTimer <= 0)
        {

            if (chocoState == ChocoState.chocoWhite)
            {
                SetAttackTimer();
                pool.SpawnBullet(playerBody.transform.forward, shootPoint.transform);
            }
            else
            {
                animator.SetTrigger("Swing");
            }
        }
    }
    void SetAttackTimer()
    {
        animator.ResetTrigger("Swing");
        attackTimer = attackTick;
    }
    void TickTimers()
    {
        if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
            speed = dashSpeed;
            rb.velocity = new Vector3(lastDirection.x, 0, lastDirection.y) * dashSpeed;
        }
        else
        {
            speed = baseSpeed;
            playerState = PlayerState.normal;
        }
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        else
        {
            Attack();
        }
    }

    public void HitEnemy(GameObject enemy)
    {
        if(chocoState == ChocoState.chocoWhite)
        {
            enemy.SetActive(false);
        }
        else
        {

        }
    }

    private void OnEnable()
    {
        _actionMap.Enable();
    }
    private void OnDisable()
    {
        _actionMap.Disable();
    }
}
