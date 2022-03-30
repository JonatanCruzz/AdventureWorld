using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(HealthUnit))]
[RequireComponent(typeof(BuffBehaviour))]
[RequireComponent(typeof(MoveBehaviour))]

public class Player : MonoBehaviour, AttackForce
{
    /*---- Variables ----*/
    private bool p_lastMovePrevent = false;
    private bool p_lastCanInteract = false;
    private bool p_canInteract = false;
    private bool _damage = false;
    private bool damage
    {
        get
        {
            return _damage;
        }
        set
        {
            anim.SetBool("damage", value);
            _damage = value;

            if (value)
            {
                this.invulnerable = true;
                StartCoroutine(onDamage());
            }
        }
    }
    private bool invulnerable = false;
    [UnityEngine.Header("Invulnerability")]
    public int AmountOfFlash = 5;
    public float InvulnerableFlashSpeed = 0.1f;
    public Color FlashColor = Color.white;
    public Color NormalColor = Color.white;

    [UnityEngine.Header("Knockback")]
    public Vector2 knockback;
    public float knockbackForce = 50;
    public int attackKnockback = 10;
    public bool attacking;
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

    [Header("Components")]
    public Ghost ghost;
    [HideInInspector] public HealthUnit hp;

    [HideInInspector] public BuffBehaviour buffs;
    private MoveBehaviour moveBehaviour;
    private PlayerInput input;
    private SpriteRenderer spriteRenderer;
    public Image barra;

    public Animator anim;
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
    [Header("Inventory")]
    [SerializeField] private Inventory defaultInventory;
    [SerializeField] private EquipmentInventory defaultEquipment;
    public InventoryUI inventoryUI;



    /*---- Variables ----*/
    private IEnumerator Flash()
    {
        for (int i = 0; i < AmountOfFlash; i++)
        {
            spriteRenderer.material.color = FlashColor;
            yield return new WaitForSeconds(InvulnerableFlashSpeed);
            spriteRenderer.material.color = NormalColor;
            yield return new WaitForSeconds(InvulnerableFlashSpeed);
        }
        this.invulnerable = false;
    }
    private IEnumerator onDamage()
    {
        yield return new WaitForSeconds(0.2f);
        this.moveBehaviour.Attack = new AttackSpecifications();
        damage = false;
        this.invulnerable = true;
        yield return StartCoroutine(Flash());
        this.invulnerable = false;


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
        Assert.IsNotNull(initialmap);
        Assert.IsNotNull(slashPrefab);
        input = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();
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
        this.inventoryUI.player = this;

        this.input.onActionTriggered += onActionTriggered;

        var TeleportUI = GameObject.FindGameObjectWithTag("TeleportUI").GetComponent<TeleportManager>();
        TeleportUI.enabled = true;
    }


    private void onActionTriggered(InputAction.CallbackContext ctx)
    {
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
                        break;
                    case InputActionPhase.Canceled:
                        moveBehaviour.StopDash();
                        break;
                }
                break;
            case "Fire":
                switch (ctx.phase)
                {
                    case InputActionPhase.Started:
                        this.movePrevent = true;
                        anim.SetTrigger("loading");
                        aura.AuraStart();
                        break;
                    case InputActionPhase.Canceled:
                        anim.SetTrigger("attacking");
                        if (aura.IsLoaded())
                        {
                            float angle = Mathf.Atan2(anim.GetFloat("movY"), anim.GetFloat("movX")) * Mathf.Rad2Deg;

                            GameObject slashObj = Instantiate(slashPrefab, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));

                            Slash slash = slashObj.GetComponent<Slash>();
                            slash.mov.x = anim.GetFloat("movX");
                            slash.mov.y = anim.GetFloat("movY");
                            if (slash.mov == Vector2.zero)
                            {
                                slash.mov = new Vector2(1, 0);
                            }
                        }
                        aura.AuraStop();
                        StartCoroutine(EnableMovementsAfter(0.4f));
                        break;
                }
                break;
        }
    }

    void Start()
    {
        Camera.main.GetComponent<CameraMovements>().setBound(initialmap);
        this.inventoryUI.gameObject.SetActive(true);
        this.canInteract = true;
    }

    void Update()
    {
        //el damage es lo que se agrego nuevo hoy 2-11-2021
        Vida();
        this.ghost.makeGhost = this.moveBehaviour.Dashing;

        if (hp.HP > 0)
        {
            if (!damage)
            {
                Animations();
                SwordAttack();
                // slashAttack();


                // if user press the button "I" to open the inventory UI
                if (Input.GetKeyDown(KeyCode.I))
                {
                    inventoryUI.Show();
                }
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
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }

    }

    public void SwordAttack()
    {

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        attacking = stateInfo.IsName("Player_Attack");

        if (Input.GetKeyDown("x") && !attacking)
        {
            anim.SetTrigger("attacking");

        }

        if (this.moveBehaviour.moveDirection != Vector2.zero) attackCollider.offset = new Vector2(this.moveBehaviour.moveDirection.x / 2, this.moveBehaviour.moveDirection.y / 2);

        if (attacking)
        {
            float playbackTime = stateInfo.normalizedTime;
            if (playbackTime > 0.33 && playbackTime < 0.66) attackCollider.enabled = true;
            else attackCollider.enabled = false;
        }
    }
    public void slashAttack()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        bool isInSlash = stateInfo.IsName("Player_Slash");

        if (isInSlash)
        {
            movePrevent = true;
        }
    }

    IEnumerator EnableMovementsAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        movePrevent = false;
    }

    /* -----------------Esto es lo nuevo que se a�adio hoy------------------------ */

    public void Finish_Damage()
    {
        damage = false;
        Debug.Log("Finish Damage");
    }

    public void Vida()
    {
        barra.fillAmount = hp.HP / hp.Max_HP;
    }

    public void Attacked(AttackSpecifications data)
    {
        if (damage || invulnerable) return;
        hp.HP -= data.damage; // TODO: apply defense
        this.damage = true;
        this.moveBehaviour.Attack = data;

        this.knockback = data.attackDirection;
        this.knockbackForce = data.knockback;

    }


    public int getAttackForce()
    {
        var attackForce = 1;
        // get equitment all calculate stats
        foreach (var item in equipment.GetAllItems())
        {
            if (item == null) continue;
            attackForce += item.strength;
        }
        Debug.Log("Attack Force: " + attackForce);
        return attackForce;
    }

    public int getKnockbackForce()
    {
        return attackKnockback;
    }
}
