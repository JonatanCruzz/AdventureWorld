using System.Collections;
using System.Collections.Generic;
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
    public float speed = 5f;

    public bool Dash;
    public float Dash_T;
    public float Speed_Dash;
    public Ghost ghost;
    private bool _damage = false;
    private bool damage
    {
        get
        {
            return _damage;
        }
        set
        {
            Debug.Log("changed damage: " + value);
            anim.SetBool("damage", value);
            _damage = value;
            if (value)
                StartCoroutine(onDamage());
        }

    }
    public Vector2 knockback;
    public float knockbackForce = 50;
    public int attackKnockback = 10;
    public HealthUnit hp;
    public BuffBehaviour buffs;

    public Image barra;

    public bool attacking;

    public Animator anim;
    Rigidbody2D rb2d;
    // Vector2 mov;

    CircleCollider2D attackCollider;

    public GameObject initialmap;
    public GameObject slashPrefab;

    public bool movePrevent;
    Aura aura;

    // Inventory

    [HideInInspector] public Inventory inventory;
    [HideInInspector] public EquipmentInventory equipment;
    [SerializeField] private Inventory defaultInventory;
    [SerializeField] private EquipmentInventory defaultEquipment;
    public InventoryUI inventoryUI;


    private MoveBehaviour moveBehaviour;
    private PlayerInput input;

    /*---- Variables ----*/

    private IEnumerator onDamage()
    {
        yield return new WaitForSeconds(0.2f);
        damage = false;
        this.moveBehaviour.Attack = new AttackSpecifications();

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

        attackCollider = transform.GetChild(0).GetComponent<CircleCollider2D>();
        attackCollider.enabled = false;

        attackCollider.GetComponent<Attack>().attackForce = this;
        aura = transform.GetChild(1).GetComponent<Aura>();


        if (!this.inventory)
        {
            this.inventory = Instantiate(defaultInventory);
        }
        if (!this.equipment)
        {
            this.equipment = Instantiate(defaultEquipment);
        }
        this.equipment.Player = this;
        this.inventoryUI.player = this;

        this.input.onActionTriggered += onActionTriggered;
    }
    private void OnEnable()
    {
        this.input.onActionTriggered += onActionTriggered;

    }
    private void OnDisable()
    {
        this.input.onActionTriggered -= onActionTriggered;
    }

    private void onActionTriggered(InputAction.CallbackContext ctx)
    {
        Debug.Log("Trigger Action:" + ctx.action.name);
        switch (ctx.action.name)
        {
            case "Move":
                moveBehaviour.Move(ctx);
                break;
        }
    }

    void Start()
    {

        Camera.main.GetComponent<CameraMovements>().setBound(initialmap);
        this.inventoryUI.gameObject.SetActive(true);

    }

    void Update()
    {
        //el damage es lo que se agrego nuevo hoy 2-11-2021
        Vida();
        Damage();

        if (hp.HP > 0)
        {
            if (!damage)
            {
                Movements();
                Animations();
                SwordAttack();
                slashAttack();
                PreventMovement();
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    Dash_Skill_Left();
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    Dash_Skill_Right();
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    Dash_Skill_Down();
                }
                else if (Input.GetKey(KeyCode.UpArrow))
                {
                    Dash_Skill_Up();
                }

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

    private void FixedUpdate()
    {
        if (hp.HP > 0)
        {
            if (!damage)
            {
                // rb2d.MovePosition(rb2d.position + mov * speed * Time.deltaTime);
            }
        }
    }

    public void Movements()
    {
        if (!Dash)
        {
            // this.moveBehaviour.moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

    }

    public void Animations()
    {
        if (this.moveBehaviour.moveDirection != Vector2.zero && !Dash)
        {
            // anim.SetFloat("movX", mov.x);
            // anim.SetFloat("movY", mov.y);
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
        bool loading = stateInfo.IsName("Player_Slash");

        if (Input.GetKeyDown(KeyCode.Z))
        {
            anim.SetTrigger("loading");
            aura.AuraStart();
        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            anim.SetTrigger("attacking");
            if (aura.IsLoaded())
            {
                float angle = Mathf.Atan2(anim.GetFloat("movY"), anim.GetFloat("movX")) * Mathf.Rad2Deg;

                GameObject slashObj = Instantiate(slashPrefab, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));

                Slash slash = slashObj.GetComponent<Slash>();
                slash.mov.x = anim.GetFloat("movX");
                slash.mov.y = anim.GetFloat("movY");
            }
            aura.AuraStop();
            StartCoroutine(EnableMovementsAfter(0.4f));
        }
        if (loading)
        {
            movePrevent = true;
        }
    }


    public void Dash_Skill_Right()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            ghost.makeGhost = true;
            Dash_T += 1 * Time.deltaTime;

            if (Dash_T < 0.35f)
            {
                Dash = true;
                transform.Translate(Vector3.right * Speed_Dash * Time.deltaTime);
            }
            else
            {
                Dash = false;

            }
        }
        else
        {
            Dash = false;
            ghost.makeGhost = false;
            Dash_T = 0;
        }
    }

    public void Dash_Skill_Left()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            ghost.makeGhost = true;
            Dash_T += 1 * Time.deltaTime;

            if (Dash_T < 0.35f)
            {
                Dash = true;

                transform.Translate(Vector3.left * Speed_Dash * Time.deltaTime);
            }
            else
            {
                Dash = false;

            }
        }
        else
        {
            Dash = false;
            ghost.makeGhost = false;
            Dash_T = 0;
        }
    }

    public void Dash_Skill_Down()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            ghost.makeGhost = true;
            Dash_T += 1 * Time.deltaTime;

            if (Dash_T < 0.35f)
            {
                Dash = true;
                transform.Translate(Vector3.down * Speed_Dash * Time.deltaTime);
            }
            else
            {
                Dash = false;
            }
        }
        else
        {
            Dash = false;
            ghost.makeGhost = false;
            Dash_T = 0;
        }
    }

    public void Dash_Skill_Up()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            ghost.makeGhost = true;
            Dash_T += 1 * Time.deltaTime;

            if (Dash_T < 0.35f)
            {
                Dash = true;
                transform.Translate(Vector3.up * Speed_Dash * Time.deltaTime);
            }
            else
            {
                Dash = false;
            }
        }
        else
        {
            Dash = false;
            ghost.makeGhost = false;
            Dash_T = 0;
        }
    }
    public void PreventMovement()
    {
        if (movePrevent)
        {
            this.moveBehaviour.moveDirection = Vector2.zero;
        }
    }
    IEnumerator EnableMovementsAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        movePrevent = false;
    }

    /* -----------------Esto es lo nuevo que se a�adio hoy------------------------ */

    public void Damage()
    {
        if (damage)
        {
            var direction = new Vector3(knockback.x, knockback.y, 0);
            var force = knockbackForce;
            // PhysicsUtils.DoMoveRigidBodyByKnockback(rb2d, this.knockback, this.knockbackForce);
        }
    }

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
        if (damage) return;
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
