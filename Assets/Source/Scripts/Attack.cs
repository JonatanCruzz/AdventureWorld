using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Animator source;
    [SerializeField]
    public AttackForce attackForce;
    public static int knockbackForce = 10;

    public void Awake()
    {
        
        if (this.attackForce == null)
        {
            if (this.GetComponent<Enemy>() is Enemy enemy)
            {
                this.attackForce = enemy;
            }
            else if (this.GetComponent<Player>() is Player player)
            {
                this.attackForce = player;
            }
            this.attackForce = this.GetComponent<AttackForce>();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // if trigger is parent of self, ignore
        if (collision.transform.IsChildOf(this.transform))
            return;
        if (this.transform.IsChildOf(collision.transform))
            return;
        var direction = new Vector2(source.GetFloat("movX"), source.GetFloat("movY"));
        var attack = new AttackSpecifications
        {
            attackDirection = direction,
            knockback = this.attackForce != null ? attackForce.getKnockbackForce() : knockbackForce,
            damage = this.attackForce != null ? attackForce.getAttackForce() : 1
        };
        if (collision.CompareTag("Enemy"))
        {

            collision.SendMessage("Attacked", attack);
            collision.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (collision.CompareTag("Player"))
        {
            if (this.attackForce != null && this.attackForce is Player) return;
            var player = collision.GetComponent<HealthUnit>();
            if (player.HP <= 0) return;

            collision.SendMessage("Attacked", attack);
            collision.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

}
