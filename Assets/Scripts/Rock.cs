using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{

    [Tooltip("Velocidad de movimiento en unidades del mundo")]
    public float speed;

    GameObject player;   // Recuperamos al objeto jugador
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
                if (collision.GetComponent<Player>().HP_min > 0)
                {
                    // collision.GetComponent<Player>().anim.SetTrigger("damage");
                    collision.GetComponent<Player>().damage = true;

                    if (transform.position.x > collision.transform.position.x)
                    {
                        collision.GetComponent<Player>().empuje = -3;
                        collision.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    else
                    {
                        collision.GetComponent<Player>().empuje = 3;
                        collision.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }

                    collision.GetComponent<Player>().HP_min -= 10;
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
