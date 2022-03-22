using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpecifications
{
    public Vector2 attackDirection;
    public float knockback;
    public int damage;

}
public class Enemy : MonoBehaviour
{

    // Variables para gestionar el radio de visión, el de ataque y la velocidad
    public float visionRadius;
    public float attackRadius;
    public float speed;
    public bool damage;
    public Vector2 empuje;
    public int knockbackForce;


    [Tooltip("Prefab de la roca que se disparará")]
    public GameObject rockPrefab;
    [Tooltip("Velocidad de ataque")]
    public float attackSpeed = 2f;
    bool attacking;

    [Tooltip("Puntos de vida")]
    public int maxHp = 50;
    [Tooltip("Vida actual")]
    public int hp;

    // Variable para guardar al jugador
    GameObject player;

    // Variable para guardar la posición inicial
    Vector3 initialPosition, target;

    // Animador y cuerpo cinemático con la rotación en Z congelada
    public Animator anim;
    public Rigidbody2D rb2d;

    void Start()
    {

        // Recuperamos al jugador gracias al Tag
        player = GameObject.FindGameObjectWithTag("Player");

        // Guardamos nuestra posición inicial
        initialPosition = transform.position;

        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        hp = maxHp;
    }

    void Update()
    {
        Damage_Enemy();
        if (!damage)
        {

            // Por defecto nuestro target siempre será nuestra posición inicial
            Vector3 target = initialPosition;

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
                anim.SetFloat("movX", dir.x);
                anim.SetFloat("movY", dir.y);
                anim.Play("Enemy_Walk", -1, 0);  // Congela la animación de andar

                if (!attacking) StartCoroutine(Attack(attackSpeed));
            }
            // En caso contrario nos movemos hacia él
            else
            {
                // apply the movement
                rb2d.MovePosition(transform.position + dir * speed * Time.deltaTime);

                // Al movernos establecemos la animación de movimiento
                anim.speed = 1;
                anim.SetFloat("movX", dir.x);
                anim.SetFloat("movY", dir.y);
                anim.SetBool("walking", true);
            }

            // Una última comprobación para evitar bugs forzando la posición inicial
            if (target == initialPosition && distance < 0.02f)
            {
                transform.position = initialPosition;
                // Y cambiamos la animación de nuevo a Idle
                anim.SetBool("walking", false);
            }

            // Y un debug optativo con una línea hasta el target
            Debug.DrawLine(transform.position, target, Color.green);

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
            Instantiate(rockPrefab, transform.position, transform.rotation);
            yield return new WaitForSeconds(seconds);
        }
        attacking = false;
    }

    public void Attacked(AttackSpecifications data)
    {
        hp -= data.damage;
        Debug.Log("hp: " + hp + " damage: " + data.damage);
        if (hp <= 0) Destroy(gameObject);
        anim.SetTrigger("damage");
        damage = true;
        var direction = data.attackDirection;
        var force = data.knockback;
        this.empuje = direction;
        this.knockbackForce = (int)force;
    }

    public void OnGUI()
    {
        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);

        GUI.Box(new Rect(pos.x - 20, Screen.height - pos.y + 60, 40, 24), hp + "/" + maxHp);

    }

    public void Damage_Enemy()
    {
        if (damage)
        {
            // move the enemy to the knockback direction
            // determine if it would collide with anything
            // if not, move the enemy
            // if it would collide, move the enemy to the closest safe spot
            var direction = new Vector3(empuje.x, empuje.y, 0);
            var force = knockbackForce;
            // create a raycast at the enemy's position in the direction of the knockback based on the force and Time.deltaTime
            // if the raycast hits the player, retry the raycast at the new position with a smaller force
            var hit = Physics2D.Raycast(transform.position, direction, force * Time.deltaTime, 1 << LayerMask.NameToLayer("Default"));
            if (hit.collider != null)
            {
                // if the raycast hits the player, retry the raycast at the new position with a smaller force
                if (hit.collider.tag == "Player")
                {
                    hit = Physics2D.Raycast(hit.collider.transform.position, direction, force * Time.deltaTime / 2, 1 << LayerMask.NameToLayer("Default"));
                }
            }

            if (hit.collider == null)
            {
                Debug.Log("No hay colision");
                rb2d.MovePosition(transform.position + direction * force * Time.deltaTime);
                // rb2d.MovePosition(transform.position + new Vector3(empuje.x, empuje.y, 0) * knockbackForce * Time.deltaTime);

            }
            else
            {
                Debug.Log("Hay colision: " + hit.collider.name + " " + hit.collider.tag + " " + hit.collider.transform.position);
                Debug.Log("Direccion: " + direction);
                Debug.Log("Force: " + force);

                var hitPoint = hit.point;
                var distance = Vector2.Distance(hitPoint, transform.position);
                var directionToMove = (hitPoint - (Vector2)transform.position).normalized;
                var distanceToMove = distance - 0.5f;
                rb2d.MovePosition(transform.position + (Vector3)directionToMove * distanceToMove * Time.deltaTime);
            }
            // GetComponent<Enemy>().enabled = false;
        }
        // GetComponent<Enemy>().enabled = true;
    }

    public void Finish_Damage_Enemy()
    {
        damage = false;
    }

}