using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(HealthUnit))]
[RequireComponent(typeof(MoveBehaviour))]
public class Enemy : MonoBehaviour, AttackForce
{
    
    // Variables para gestionar el radio de visión, el de ataque y la velocidad
    public float visionRadius;
    public float attackRadius;
    public bool damage;

    public int attackDamage = 1;
    public int attackKnockback = 10;


    [Tooltip("Prefab de la roca que se disparará")]
    public GameObject rockPrefab;
    [Tooltip("Velocidad de ataque")]
    public float attackSpeed = 2f;
    bool attacking;

    public HealthUnit hp;

    // Variable para guardar al jugador
    GameObject player;

    // Variable para guardar la posición inicial
    public Vector3 initialPosition;
    public Vector3 target;

    // Animador y cuerpo cinemático con la rotación en Z congelada
    public Animator anim;
    public Rigidbody2D rb2d;
    private MoveBehaviour moveBehaviour;

    void Start()
    {

        // Recuperamos al jugador gracias al Tag
        player = GameObject.FindGameObjectWithTag("Player");
        hp = GetComponent<HealthUnit>();

        // Guardamos nuestra posición inicial
        initialPosition = transform.position;

        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        moveBehaviour = GetComponent<MoveBehaviour>();

        // hp = maxHp;
    }
    private void CalculateMove()
    {
        // Por defecto nuestro target siempre será nuestra posición inicial
        target = initialPosition;

        // Comprobamos un Raycast del enemigo hasta el jugador
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            player.transform.position - transform.position,
            visionRadius,
            1 << LayerMask.NameToLayer("Default")
        // Poner el propio Enemy en una layer distinta a Default para evitar el raycast
        // También poner al objeto Attack y al Prefab Slash una Layer Attack 
        // Sino los detectará como entorno y se mueve atrás al hacer ataques
        );

        // Aquí podemos debugear el Raycast
        Vector3 forward = transform.TransformDirection(player.transform.position - transform.position);
        Debug.DrawRay(transform.position, forward, Color.red);

        // Si el Raycast encuentra al jugador lo ponemos de target
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Player")
            {
                target = player.transform.position;
            }
        }

        // Calculamos la distancia y dirección actual hasta el target
        float distance = Vector3.Distance(target, transform.position);
        Vector3 dir = (target - transform.position).normalized;

        // Si es el enemigo y está en rango de ataque nos paramos y le atacamos
        if (target != initialPosition && distance < attackRadius)
        {
            // Aquí le atacaríamos, pero por ahora simplemente cambiamos la animación

            anim.Play("Enemy_Walk", -1, 0);  // Congela la animación de andar
            moveBehaviour.moveDirection = Vector2.zero;

            if (!attacking) StartCoroutine(Attack(attackSpeed));
        }
        // En caso contrario nos movemos hacia él
        else
        {
            moveBehaviour.moveDirection = dir;
            anim.SetBool("walking", true);
        }

        // Una última comprobación para evitar bugs forzando la posición inicial
        if (target == initialPosition && distance < 0.02f)
        {
            transform.position = initialPosition;
            moveBehaviour.moveDirection = Vector2.zero;
            // Y cambiamos la animación de nuevo a Idle
            anim.SetBool("walking", false);
        }

        // Y un debug optativo con una línea hasta el target
        Debug.DrawLine(transform.position, target, Color.green);

    }


    void Update()
    {
        if (!damage)
        {
            CalculateMove();

        }
    }
    // Podemos dibujar el radio de visión y ataque sobre la escena dibujando una esfera
    void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
        Gizmos.DrawWireSphere(transform.position, attackRadius);

    }

    IEnumerator Attack(float seconds)
    {
        attacking = true;
        if (target != initialPosition && rockPrefab != null)
        {
            var rock = Instantiate(rockPrefab, transform.position, transform.rotation);
            rock.GetComponent<Rock>().source = this;
            yield return new WaitForSeconds(seconds);
        }
        attacking = false;
    }

    public void Attacked(AttackSpecifications data)
    {
        if (!this.enabled) return;
        hp.HP -= data.damage;
        if (hp.HP <= 0) Destroy(gameObject);
        anim.SetTrigger("damage");
        damage = true;
        this.moveBehaviour.Attack = data;
        StartCoroutine(OnDamage());
    }

    private IEnumerator OnDamage()
    {
        yield return new WaitForSeconds(0.2f);
        damage = false;
        this.moveBehaviour.Attack = new AttackSpecifications();

    }

    public void OnGUI()
    {
        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);

        GUI.Box(new Rect(pos.x - 20, Screen.height - pos.y + 60, 40, 24), hp.HP + "/" + hp.Max_HP);

    }


    public void Finish_Damage_Enemy()
    {
        damage = false;

    }

    public int getAttackForce()
    {
        return attackDamage;
    }

    public int getKnockbackForce()
    {
        return attackKnockback;
    }
}