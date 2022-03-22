using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Player player;
    public int knockbackForce = 50;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<Enemy>();
            var direction = new Vector2(player.anim.GetFloat("movX"), player.anim.GetFloat("movY"));
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
