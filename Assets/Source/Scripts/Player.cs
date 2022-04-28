using System;
using System.Collections;
using AdventureWorld.Prueba;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Animancer;
using Com.LuisPedroFonseca.ProCamera2D;
using DG.Tweening;
using PixelCrushers;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(HealthUnit))]
[RequireComponent(typeof(BuffBehaviour))]
[RequireComponent(typeof(MoveBehaviour))]
[System.Serializable]
public class Player : MonoBehaviour, AttackForce
{
    /*---- Variables ----*/
    private bool p_lastMovePrevent = false;
    private bool p_lastCanInteract = false;
    private bool p_canInteract = false;
    private bool damage;
    private bool invulnerable = false;

    [UnityEngine.Header("Invulnerability")]
    public int AmountOfFlash = 5;

    public float InvulnerableFlashSpeed = 0.1f;
    public Color FlashColor = Color.white;
    public Color NormalColor = Color.white;

    [UnityEngine.Header("Attack")] public int attackKnockback = 10;
    public int baseAttack = 1;
    public bool attacking;
    private AnimancerState attackState;

    public bool canInteract
    {
        get => p_canInteract;
        set
        {
            this.p_canInteract = value;
            if (!value)
                this.moveBehaviour.moveDirection = Vector2.zero;
        }
    }

    [Header("Animation")]
    // public Animator anim;
    [HideInInspector]
    public HybridAnimancerComponent animancer;

    [SerializeField] private AnimationClip _DamageClip;
    [SerializeField] private MixerTransition2DAsset.UnShared _AttackTransition;
    private PlayerState state;
    [Header("Components")] public Ghost ghost;
    [HideInInspector] public HealthUnit hp;

    [HideInInspector] public BuffBehaviour buffs;
    private MoveBehaviour moveBehaviour;
    private PlayerInput input;
    private SpriteRenderer spriteRenderer;
    public Image barra;

    public AudioSource src;
    public AudioClip atk;
    public AudioClip dash;
    public AudioClip l_atk;
    public AudioClip l_atk_shot;
    public AudioClip ouch;
    public AudioClip interact;

    [HideInInspector] Rigidbody2D rb2d;

    CircleCollider2D attackCollider;

    public GameObject initialmap;
    public GameObject slashPrefab;
    Aura aura;

    public bool movePrevent
    {
        get => this.moveBehaviour.movePrevent;
        set => this.moveBehaviour.movePrevent = value;
    }

    // Inventory

    [HideInInspector] public Inventory inventory;
    [HideInInspector] public EquipmentInventory equipment;
    [Header("Inventory")] [SerializeField] private Inventory defaultInventory;
    [SerializeField] private EquipmentInventory defaultEquipment;
    public InventoryUI inventoryUI;

    public UnityEvent onCloseMenu;
    public UnityEvent<InputAction.CallbackContext> onUnhandledInput;

    private Interactable _interactable;

    public Interactable interactable
    {
        get => _interactable;
        set
        {
            if (_interactable != null)
                _interactable.HideKey();
            _interactable = value;
            if (_interactable != null)
                _interactable.ShowKey(this);
        }
    }

    /*---- Variables ----*/
    private IEnumerator Flash()
    {
        this.invulnerable = true;
        for (int i = 0; i < AmountOfFlash; i++)
        {
            spriteRenderer.material.color = FlashColor;
            yield return new WaitForSeconds(InvulnerableFlashSpeed);
            spriteRenderer.material.color = NormalColor;
            yield return new WaitForSeconds(InvulnerableFlashSpeed);
        }

        spriteRenderer.material.color = NormalColor;

        this.invulnerable = false;
    }

    private IEnumerator onDamage()
    {
        this.damage = true;
        yield return this.animancer.Play(_DamageClip);
        this.animancer.Play(this.state);

        this.moveBehaviour.Attack = new AttackSpecifications();
        damage = false;
        yield return StartCoroutine(Flash());
    }

    private void OnDisable()
    {
        this.p_lastMovePrevent = this.movePrevent;
        this.p_lastCanInteract = this.canInteract;
        this.movePrevent = true;
        this.input.onActionTriggered -= onActionTriggered;
        this.canInteract = false;
    }

    private void OnEnable()
    {
        this.input.onActionTriggered += onActionTriggered;
        this.canInteract = this.p_lastCanInteract;
        this.movePrevent = this.p_lastMovePrevent;
    }

    private void Awake()
    {
        // Assert.IsNotNull(initialmap);
        Assert.IsNotNull(slashPrefab);
        input = GetComponent<PlayerInput>();
        // anim = GetComponent<Animator>();
        animancer = GetComponent<HybridAnimancerComponent>();
        rb2d = GetComponent<Rigidbody2D>();
        hp = GetComponent<HealthUnit>();
        buffs = GetComponent<BuffBehaviour>();

        moveBehaviour = GetComponent<MoveBehaviour>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        attackCollider = transform.GetChild(0).GetComponent<CircleCollider2D>();
        attackCollider.enabled = false;

        attackCollider.GetComponent<Attack>().attackForce = this;
        aura = transform.GetChild(1).GetComponent<Aura>();


        if (!this.inventory)
            this.inventory = Instantiate(defaultInventory);
        if (!this.equipment)
            this.equipment = Instantiate(defaultEquipment);

        this.equipment.Player = this;
        GameController.Instance.uiManager.player = this;

        this.input.onActionTriggered += onActionTriggered;

        this.state = new PlayerState(animancer.Controller);
        ProCamera2D.Instance.AddCameraTarget(transform);
    }
    



    private void onActionTriggered(InputAction.CallbackContext ctx)
    {
        
        if (GameController.Instance != null && GameController.Instance.IsPaused)
            return;


        switch (ctx.action.name)
        {
            case "Move":
                moveBehaviour.Move(ctx);
                break;

            case "Dash":
                if (ctx.phase == InputActionPhase.Canceled) return;

                switch (ctx.phase)
                {
                    case InputActionPhase.Started:
                        moveBehaviour.Dash();
                        src.clip = dash;
                        src.Play();
                        break;
                    case InputActionPhase.Canceled:
                        moveBehaviour.StopDash();
                        break;
                }

                break;
            case "Attack":

                switch (ctx.phase)
                {
                    case InputActionPhase.Started:
                        if (attacking) return;
                        StartCoroutine(this.DoAttack());
                        src.clip = atk;
                        src.Play();
                        break;
                    case InputActionPhase.Canceled:
                        // attackCollider.enabled = false;
                        break;
                }

                break;
            case "Interact":
                if (ctx.phase == InputActionPhase.Started)
                {
                    if (this.canInteract && this.interactable != null)
                    {
                        src.clip = interact;
                        src.Play();
                        this.interactable.DoClick(this);
                    }
                }

                break;
            case "Fire":
                slashAttack(ctx);
                break;

            // case "CloseMenu":
            //     if (ctx.phase == InputActionPhase.Started)
            //     {
            //         this.onCloseMenu?.Invoke();
            //     }
            //
            //     break;
            

            default:
                this.onUnhandledInput?.Invoke(ctx);
                break;
        }
    }

    private IEnumerator DoAttack()
    {
        this.attacking = true;
        this.attackState = this.animancer.Play(_AttackTransition);
        _AttackTransition.State.Parameter = this.moveBehaviour.moveDirection;
        // animState.Time = 0;

        yield return attackState;
        // this.animancer.Stop();
        this.animancer.Play(this.state);

        this.attacking = false;
    }

    void Start()
    {
        animancer.Play(state);
        // Camera.main.GetComponent<CameraMovements>().setBound(initialmap);
        // this.inventoryUI.gameObject.SetActive(true);
        this.canInteract = true;
        
        if ( SaveSystem.playerSpawnpoint != null)
        {
            this.transform.position = SaveSystem.playerSpawnpoint.transform.position;
            this.transform.rotation = SaveSystem.playerSpawnpoint.transform.rotation;
            
        }
    }

    public void Load(string scene)
    {
        PixelCrushers.SaveSystem.LoadScene(scene);
    }

    public void Kill()
    {
        this.hp.HP = 0;
    }

    void Update()
    {
        //el damage es lo que se agrego nuevo
        Vida();
        this.ghost.makeGhost = this.moveBehaviour.Dashing;

        if (hp.HP > 0)
        {
            if (!damage)
            {
                Animations();
                SwordAttack();

                // // if user press the button "I" to open the inventory UI
                // if (Input.GetKeyDown(KeyCode.I))
                // {
                //     inventoryUI.Show();
                // }
            }
        }
        else
        {
            GameController.Instance.audio.Stop();
            
            hp.HP = hp.Max_HP;
            //get the loaded scene
            var scene = SceneManager.GetActiveScene();
            // if the scene is "SampleScene", load the scene "NuevoMap 1"
            if (scene.name == "SampleScene")
            {
                //GameController.Instance.UI.SetActive(false);
                Destroy(GameController.Instance.UI);
                Destroy(GameController.Instance.th);
                Destroy(GameController.Instance);
                PixelCrushers.SaveSystem.LoadScene("Muerte");
            }
        }
        /* else
         {
             switch (dead)
             {
                 case 0:
                     anim.SetTrigger("dead");
                     dead++;
                     break;
             }
         }*/


        // hide teleport dialog if the player press the space key
    }

    public void Animations()
    {
        if (this.moveBehaviour.moveDirection != Vector2.zero && !this.moveBehaviour.Dashing)
        {
            this.state.Walking = true;
        }
        else
        {
            this.state.Walking = false;
        }
    }

    public void SwordAttack()
    {
        if (this.moveBehaviour.moveDirection != Vector2.zero)
            attackCollider.offset = new Vector2(this.moveBehaviour.moveDirection.x / 2,
                this.moveBehaviour.moveDirection.y / 2);

        if (attacking && this.attackState != null)
        {
            float playbackTime = this.attackState.NormalizedTime;
            if (playbackTime > 0.33 && playbackTime < 0.66) attackCollider.enabled = true;
            else attackCollider.enabled = false;
        }
    }

    public void slashAttack(InputAction.CallbackContext ctx)
    {
        if (this.inSlash) return;
        switch (ctx.phase)
        {
            case InputActionPhase.Started:
                this.p_lastMovePrevent = this.movePrevent;
                this.movePrevent = true;
                // this.state.lo
                this.state.Setloading();
                aura.AuraStart();
                src.clip = l_atk;
                src.Play();
                break;
            case InputActionPhase.Canceled:
                src.clip = l_atk_shot;
                src.Play();
                StartCoroutine(DoingSlashAttack());
                break;
        }
    }

    private bool inSlash = false;

    private IEnumerator DoingSlashAttack()
    {
        this.inSlash = true;
        this.movePrevent = true;
        this.state.Setattacking();
        if (aura.IsLoaded())
        {
            float angle = Mathf.Atan2(this.state.movY, this.state.movX) * Mathf.Rad2Deg;

            GameObject slashObj = Instantiate(slashPrefab, transform.position,
                Quaternion.AngleAxis(angle, Vector3.forward));

            Slash slash = slashObj.GetComponent<Slash>();
            slash.mov.x = this.state.movX;
            slash.mov.y = this.state.movY;
            if (slash.mov == Vector2.zero)
            {
                slash.mov = new Vector2(1, 0);
            }
        }

        aura.AuraStop();
        yield return new WaitForSeconds(0.4f);
        this.movePrevent = this.p_lastMovePrevent;
        this.inSlash = false;
    }


    /* -----------------Esto es lo nuevo que se a�adio hoy------------------------ */


    public void Vida()
    {
        barra.fillAmount = hp.HP / hp.Max_HP;
    }

    public void Attacked(AttackSpecifications data)
    {
        if (damage || invulnerable) return;
        src.clip = ouch;
        src.Play();
        hp.HP -= data.damage; // TODO: apply defense
        this.moveBehaviour.Attack = data;
        StartCoroutine(onDamage());
    }


    public int getAttackForce()
    {
        var attackForce = this.baseAttack;
        // get equitment all calculate stats
        foreach (var item in equipment.GetAllItems())
        {
            if (item == null) continue;
            attackForce += item.strength;
        }

        return attackForce;
    }

    public int getKnockbackForce()
    {
        return attackKnockback;
    }

    public int money = 0;

    public void AddGold(GoldCoin coin)
    {
        money += coin.value;

        // //use Dotween to animate the coin to the player
        // coin.transform.DOMove(transform.position, 0.1f);
        // //destroy the coin
        // Destroy(coin.gameObject, 0.1f);
    }
}