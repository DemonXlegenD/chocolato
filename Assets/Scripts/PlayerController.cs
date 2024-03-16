
using NovaSamples.UIControls;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using Nova;

public class PlayerController : MonoBehaviour
{
    enum PlayerState { normal, dashing, hitted };
    enum ChocoState { chocoWhite, chocoBlack, chocoMilk };

    [Header("Player Input")]
    private PlayerInput _playerInput;
    private InputActionAsset _inputActions;
    private InputActionMap _actionMap;

    [Header("Player Stats")]
    [SerializeField] float life;
    [SerializeField] int speed;
    [SerializeField] int baseSpeed;
    [SerializeField] int damage;
    [SerializeField] float dashTimer;
    [SerializeField] float dashBaseTime;
    [SerializeField] int dashSpeed;
    [SerializeField] Vector2 lastDirection;
    [SerializeField] float colorState; // jauge
    [SerializeField] float colorEvolve; // ce que tu gagnes ou perds
    [SerializeField] float colorTimer; // timer qui met a jour la jauge couleur
    [SerializeField] float colorTick; //base time 
    [SerializeField] float attackTimer; //timer attack


    [Header("Player Weapons")]
    [SerializeField] float switchWeaponTime = 5;
    [SerializeField] int whiteWeaponLevel = 1;
    [SerializeField] float whiteWeaponXpActual;
    [SerializeField] float whiteWeaponXpMax;
    [SerializeField] float whiteWeaponDmg;
    [SerializeField] float whiteWeaponBaseDmg = 10;
    [SerializeField] float whiteAttackTick;
    [SerializeField] int blackWeaponLevel = 1;
    [SerializeField] float blackWeaponXpActual;
    [SerializeField] float blackWeaponXpMax;
    [SerializeField] float blackWeaponDmg;
    [SerializeField] float blackWeaponBaseDmg = 20;
    [SerializeField] float blackAttackTick;

    [Header("Materials")]
    [SerializeField] MeshRenderer playerMesh;
    [SerializeField] Material whiteMat;
    [SerializeField] Material blackMat;

    [Header("State")]
    [SerializeField] PlayerState playerState;
    [SerializeField] ChocoState chocoState; //

    [Header("Components")]
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject playerBody;
    [SerializeField] Animator animator;


    [Header("Shoot")]
    [SerializeField] GameObject shootPoint;
    [SerializeField] GameObject prefabBullet;
    [SerializeField] PoolObjects pool;

    [Header("UI")]
    [SerializeField] Slider ChocolatJauge;
    [SerializeField] WeaponsDisplay DarkChocolateWeaponDisplay;
    [SerializeField] WeaponsDisplay WhiteChocolateWeaponDisplay;
    [SerializeField] TextMeshPro levelDarkWeapon;
    [SerializeField] TextMeshPro levelWhiteWeapon;
    [SerializeField] Slider xpWhiteWeapon;
    [SerializeField] Slider xpDarkWeapon;
    [SerializeField] Slider hpPlayer;
    [SerializeField] LoadDash dashSlider;
    [SerializeField] MenuPause menuPause;
    [SerializeField] TextMeshPro expText;
    [SerializeField] UIBlock center;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip musicChill;
    [SerializeField] AudioClip musicVnr;

    [Header("Others")]
    [SerializeField] Color whiteColor;
    [SerializeField] Color blackColor;

    private bool canSwitchWeapon = false;
    private bool canDash = false;

    // Start is called before the first frame update
    void Awake()
    {
        playerMesh = GetComponentInChildren<MeshRenderer>();
        playerMesh.material = whiteMat;
        rb = GetComponent<Rigidbody>();
        shootPoint = GameObject.FindWithTag("ShootPoint");
        _playerInput = GetComponent<PlayerInput>();
        _inputActions = _playerInput.actions;
        _actionMap = _inputActions.FindActionMap("Player");
        ChocolatJauge.Value = colorState;
        animator = GetComponent<Animator>();
        blackWeaponDmg = blackWeaponBaseDmg;
        whiteWeaponDmg = whiteWeaponBaseDmg;
        xpWhiteWeapon.Value = whiteWeaponXpActual;
        xpWhiteWeapon.Max = whiteWeaponXpMax;
        xpDarkWeapon.Value = blackWeaponXpActual;
        xpDarkWeapon.Max = blackWeaponXpMax;
        hpPlayer.Value = life;
    }

    // Update is called once per frame
    void Update()
    {
        if (_actionMap.FindAction("Pause").WasPressedThisFrame())
        {
            if (!menuPause.IsPause) menuPause.PauseGame();
            else menuPause.ResumeGame();
        }
        if (playerState == PlayerState.normal || playerState == PlayerState.hitted)
        {
            Rotate();
            if (_actionMap.FindAction("Dash").WasPressedThisFrame() && canDash)
            {
                Dash();
            }
        }

        SwapColor();

        TickTimers();

        SetColor();

        SetArms();
    }

    private void SetArms()
    {
        levelWhiteWeapon.text = whiteWeaponLevel.ToString();
        levelDarkWeapon.text = blackWeaponLevel.ToString();
    }

    private void SwapColor()
    {
        if (canSwitchWeapon)
        {
            if (_actionMap.FindAction("SwapColor").WasPressedThisFrame())
            {
                if (chocoState == ChocoState.chocoWhite)
                {
                    playerMesh.material = blackMat;
                    chocoState = ChocoState.chocoBlack;
                    colorState -= colorEvolve;
                }
                else
                {
                    playerMesh.material = whiteMat;
                    chocoState = ChocoState.chocoWhite;
                    colorState += colorEvolve;
                }
                DarkChocolateWeaponDisplay.ChangeDisplay();
                WhiteChocolateWeaponDisplay.ChangeDisplay();
                canSwitchWeapon = false;
                colorTimer = colorTick;
                SetAttackTimer();
            }
        }

    }

    void FixedUpdate()
    {
        if (playerState == PlayerState.normal || playerState == PlayerState.hitted)
        {
            Move();
        }
    }


    #region Getter

    public float GetSwitchWeaponTime() { return switchWeaponTime; }

    #endregion

    public void CanSwitchWeapon() { canSwitchWeapon = true; }
    public void CanDash() { canDash = true; }

    private void Move()
    {
        Vector3 moveInput = _actionMap.FindAction("Move").ReadValue<Vector2>();
        moveInput.Normalize();
        if (moveInput != Vector3.zero)
        {
            lastDirection = moveInput;
            rb.velocity = new Vector3(moveInput.x, 0, moveInput.y) * speed;
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
        dashSlider.Reset();
        dashTimer = dashBaseTime;
        playerState = PlayerState.dashing;
        canDash = false;
    }
    void SetColor()
    {
        if (colorTimer <= 0 && colorState > 0 && colorState < 100)
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
        ChocolatJauge.Value = colorState;
    }
    void Attack()
    {
        if (attackTimer <= 0)
        {

            if (chocoState == ChocoState.chocoWhite)
            {
                SetAttackTimer();
                
                pool.SpawnPlayerBullet(playerBody.transform.forward, shootPoint.transform);
            }
            else
            {
                animator.SetTrigger("Swing");
            }
        }
    }
    void SetAttackTimer()
    {
        if (chocoState == ChocoState.chocoWhite)
        {
            attackTimer = whiteAttackTick;
        }
        else
        {
            attackTimer = blackAttackTick;
            animator.ResetTrigger("Swing");
        }
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

    public void HitEnemy(GameObject enemy, GameObject bullet = null)
    {
        var tempBhv = enemy.GetComponent<EnemyBehaviour>();
        if (chocoState == ChocoState.chocoBlack)
        {
            switch (tempBhv.enemyColor)
            {
                case EnemyBehaviour.EnemyColor.chocoWhite:
                    Debug.Log("oui");
                    tempBhv.TakeDamage(blackWeaponDmg);
                    enemy.GetComponent<Rigidbody>().AddForce((enemy.transform.position - transform.position).normalized * 50, ForceMode.Impulse);
                    break;
                case EnemyBehaviour.EnemyColor.any:
                    tempBhv.TakeDamage(blackWeaponDmg);
                    break;
            }
        }
        else
        {
            switch (tempBhv.enemyColor)
            {
                case EnemyBehaviour.EnemyColor.chocoBlack:
                    Debug.Log("oui");
                    tempBhv.TakeDamage(whiteWeaponDmg);
                    enemy.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * 5, ForceMode.Impulse);
                    break;
                case EnemyBehaviour.EnemyColor.any:
                    tempBhv.TakeDamage(whiteWeaponDmg);
                    break;
            }
        }
    }

    public void GetDamaged(float damage)
    {
        life -= damage;
        hpPlayer.Value = life;
    }

    void GetHealed(float heal)
    {
        life += heal;
    }

    void OnEnemyDeath(EnemyBehaviour.EnemyColor color)
    {
        if (color == EnemyBehaviour.EnemyColor.chocoWhite)
        {
            whiteWeaponXpActual += 5;
        }
        else if (color == EnemyBehaviour.EnemyColor.chocoBlack)
        {
            blackWeaponXpActual += 5;
        }
        xpWhiteWeapon.Value = whiteWeaponXpActual;
        xpWhiteWeapon.Max = whiteWeaponXpMax;
        xpDarkWeapon.Value = blackWeaponXpActual;
        xpDarkWeapon.Max = blackWeaponXpMax;
        CheckLevelUpWeapon();
    }

    public void AddExp(EnemyBehaviour.EnemyColor color)
    {
        switch (color)
        {
            case EnemyBehaviour.EnemyColor.chocoWhite:
                whiteWeaponXpActual += 20;
                var temp  = Instantiate(expText, center.transform);
                temp.text = "+ 20 white xp";
                temp.color = whiteColor;
                break;
            case EnemyBehaviour.EnemyColor.chocoBlack:
                blackWeaponXpActual += 20;
                var temp2  = Instantiate(expText, center.transform);
                temp2.text = "+ 20 black xp";
                temp2.color = blackColor;
                break;
            case EnemyBehaviour.EnemyColor.any:
                whiteWeaponXpActual += 100;
                blackWeaponXpActual += 100;
                break;
        }
    }
    void CheckLevelUpWeapon()
    {
        if (whiteWeaponXpActual >= whiteWeaponXpMax)
        {
            whiteWeaponLevel++;
            whiteWeaponXpActual = whiteWeaponXpActual - whiteWeaponXpMax;
            whiteWeaponXpMax += 30;
            xpWhiteWeapon.Max = whiteWeaponXpMax;
            whiteAttackTick -= whiteAttackTick * 15 / 100;
            whiteWeaponDmg += whiteWeaponBaseDmg * (whiteWeaponLevel - 1);
        }
        else if (blackWeaponXpActual >= blackWeaponXpMax)
        {
            blackWeaponLevel++;
            blackWeaponXpActual = blackWeaponXpActual - blackWeaponXpMax;
            blackWeaponXpMax += 30;
            xpDarkWeapon.Max = blackWeaponXpMax;
            blackAttackTick -= blackAttackTick * 15 / 100;
            blackWeaponDmg += blackWeaponBaseDmg * (blackWeaponLevel - 1);
        }
    }

    void CheckDeath()
    {
        if(life <= 0 || colorState <= 0 || colorState >= 100)
        {

        }
    }
    private void OnEnable()
    {
        _actionMap.Enable();
        EventManager.GetInstance().onEnemyDeath.AddListener(OnEnemyDeath);
    }

    private void OnDisable()
    {
        _actionMap.Disable();
        EventManager.GetInstance().onEnemyDeath.RemoveListener(OnEnemyDeath);
    }
}