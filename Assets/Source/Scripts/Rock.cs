using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{

    [Tooltip("Velocidad de movimiento en unidades del mundo")]
    public float speed;

    GameObject player;   // Recuperamos al objeto jugador
    public AttackForce source;  // Recuperamos al objeto fuente de ataque
    Rigidbody2D rb2d;    // Recuperamos el componente de cuerpo rígido
    Vector3 target, dir; // Vectores para almacenar el objetivo y su dirección

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb2d = GetComponent<Rigidbody2D>();

        // Recuperamos posición del jugador y la dirección normalizada
        if (player != null)
        {
            target = player.transform.position;
            dir = (target - transform.position).normalized;
        }
    }

    void FixedUpdate()
    {
        // Si hay un objetivo movemos la roca hacia su posición
        if (target != Vector3.zero)
        {
            rb2d.MovePosition(transform.position + (dir * speed) * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Si chocamos contra el jugador o un ataque la borramos

        if (collision.transform.tag == "Player" || collision.transform.tag == "Attack")
        {
            Destroy(gameObject);

            //Esto es lo nuevo que se agrego hoy 2-11-2021
            if (collision.CompareTag("Player"))
            {
                if (collision.GetComponent<HealthUnit>().HP > 0)
                {
                    var attack = new AttackSpecifications
                    {
                        attackDirection = dir,
                        knockback = this.source?.getKnockbackForce() ?? Attack.knockbackForce,
                        damage = this.source?.getAttackForce() ?? 10
                    };
                    collision.SendMessage("Attacked", attack);
                }

            }
        }
    }

    void OnBecameInvisible()
    {
        // Si se sale de la pantalla borramos la roca
        Destroy(gameObject);
    }
}
