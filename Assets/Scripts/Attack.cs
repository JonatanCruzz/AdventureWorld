using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Animator source;
    public int knockbackForce = 50;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<Enemy>();
            var direction = new Vector2(source.GetFloat("movX"), source.GetFloat("movY"));
            var attack = new AttackSpecifications
            {
                attackDirection = direction,
                knockback = knockbackForce,
                damage = 1
            };
            collision.SendMessage("Attacked", attack);
            collision.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

}
