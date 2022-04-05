using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AdventureWorld.Prueba.Enemy
{
    [RequireComponent(typeof(MoveBehaviour), typeof(HealthUnit))]
    public class GenericEnemy : MonoBehaviour, AttackForce
    {
        public BaseEnemy baseEnemy;
        public Spawner spawner;
        [ReadOnly] public bool damage;

        [ReadOnly] public bool canAttack = true;

        bool attacking;

        public HealthUnit hp;

        // Variable para guardar al jugador
        GameObject player;

        // Variable para guardar la posición inicial
        public Vector3 initialPosition;
        public Vector3 target;

        // Animador y cuerpo cinemático con la rotación en Z congelada
        public AnimancerComponent anim;
        private MoveBehaviour moveBehaviour;

        void Start()
        {
            // Recuperamos al jugador gracias al Tag
            player = GameObject.FindGameObjectWithTag("Player");
            hp = GetComponent<HealthUnit>();
            hp.BaseHP = baseEnemy.health;
            hp.AddictiveHP = 0;
            // Guardamos nuestra posición inicial
            initialPosition = transform.position;

            anim = GetComponent<AnimancerComponent>();
            moveBehaviour = GetComponent<MoveBehaviour>();
            this.Init();
            // hp = maxHp;
        }

        public void Init()
        {
            this.hp.HP = this.hp.BaseHP;
            this.moveBehaviour.disableAnimation = true;
        }

        private void CalculateMove()
        {
            if (attacking) return;
            // Por defecto nuestro target siempre será nuestra posición inicial
            target = initialPosition;

            // Comprobamos un Raycast del enemigo hasta el jugador
            var hit = Physics2D.Raycast(
                transform.position,
                player.transform.position - transform.position,
                baseEnemy.visionRange,
                1 << LayerMask.NameToLayer("Default")
                // Poner el propio Enemy en una layer distinta a Default para evitar el raycast
                // También poner al objeto Attack y al Prefab Slash una Layer Attack 
                // Sino los detectará como entorno y se mueve atrás al hacer ataques
            );

            // Aquí podemos debugear el Raycast
            // var forward = transform.TransformDirection(player.transform.position - transform.position);
            // Debug.DrawRay(transform.position, forward, Color.red);

            // Si el Raycast encuentra al jugador lo ponemos de target
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Player")
                {
                    target = player.transform.position;
                }
            }

            // Calculamos la distancia y dirección actual hasta el target
            var distance = Vector3.Distance(target, transform.position);
            var dir = (target - transform.position).normalized;
            // Si es el enemigo y está en rango de ataque nos paramos y le atacamos
            if (target != initialPosition && distance < baseEnemy.attackDefinition.range)
            {
                // Aquí le atacaríamos, pero por ahora simplemente cambiamos la animación
                // Congela la animación de andar
                moveBehaviour.moveDirection = Vector2.zero;
                if (canAttack)
                {
                    Debug.Log("Attack");
                    StartCoroutine(Attack(dir, baseEnemy.attackDefinition.speed));
                }
            }
            // En caso contrario nos movemos hacia él
            else
            {
                moveBehaviour.moveDirection = dir;
                anim.Play(baseEnemy.moveAnimation.GetClip(dir));
                // anim.Play()
                // anim.SetBool("walking", true);
            }

            // Una última comprobación para evitar bugs forzando la posición inicial
            if (target == initialPosition && distance < 0.1f)
            {
                transform.position = initialPosition;
                moveBehaviour.moveDirection = Vector2.zero;
                // Y cambiamos la animación de nuevo a Idle
                this.anim.Play(baseEnemy.idleAnimation);
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
            Gizmos.DrawWireSphere(transform.position, baseEnemy.visionRange);
            Gizmos.DrawWireSphere(transform.position, baseEnemy.attackDefinition.speed);
        }

        private IEnumerator DoChargeAttack(Vector2 dir)
        {
            if (baseEnemy.attackDefinition.chargeTime > 0)
            {
                if (baseEnemy.attackChargeAnimation != null)
                {
                    anim.Play(baseEnemy.attackChargeAnimation
                        .GetClip(dir));
                    // TODO: change animation dynamically while enemy move
                    //TODO: implement basEnemy.canMoveWhileAttacknig
                }

                yield return new WaitForSeconds(baseEnemy.attackDefinition.chargeTime);
            }

            yield break;
        }

        IEnumerator Attack(Vector2 dir, float seconds)
        {
            attacking = true;
            canAttack = false;
            AnimancerState state = null;
            yield return StartCoroutine(DoChargeAttack(dir));

            if (baseEnemy.attackFreezeAnimation)
            {
                anim.Playable.PauseGraph();
                // anim.Stop();
            }
            else
            {
                state = anim.Play(baseEnemy.attackAnimation.GetClip(dir));
            }

            if (target != initialPosition && baseEnemy.attackDefinition.prefab != null)
            {
                for (var i = 0; i < baseEnemy.attackDefinition.instaceAmount; i++)
                {
                    var rock = Instantiate(baseEnemy.attackDefinition.prefab, transform.position, transform.rotation);
                    var rockScript = rock.GetComponent<Rock>();
                    rockScript.source = this;
                    if (baseEnemy.attackDefinition.modifyProjectileSpeed)
                        rockScript.speed = baseEnemy.attackDefinition.projectileSpeed;
                    if (baseEnemy.attackDefinition.reloadPerInstance)
                        yield return StartCoroutine(DoChargeAttack(dir));
                }

                yield return new WaitForSeconds(seconds);
            }

            if (state != null)
                yield return state;

            if (baseEnemy.attackFreezeAnimation)
            {
                anim.Playable.UnpauseGraph();
                // anim.Stop();
            }


            attacking = false;
            yield return new WaitForSeconds(baseEnemy.attackDefinition.cooldown);
            canAttack = true;
        }


        public void Attacked(AttackSpecifications data)
        {
            if (!this.enabled) return;
            hp.HP -= data.damage;
            if (hp.HP <= 0)
            {
                this.spawner.RemoveEnemy(this);
            }


            StartCoroutine(OnDamage(data));
        }

        private IEnumerator OnDamage(AttackSpecifications data)
        {
            damage = true;
            this.moveBehaviour.Attack = data;
            yield return anim.Play(baseEnemy.damagedAnimation);
            anim.Play(baseEnemy.idleAnimation);
            damage = false;
            this.moveBehaviour.Attack = new AttackSpecifications();
        }

        public void OnGUI()
        {
            Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);

            GUI.Box(new Rect(pos.x - 20, Screen.height - pos.y + 60, 40, 24), hp.HP + "/" + hp.Max_HP);
        }


        public int getAttackForce()
        {
            return baseEnemy.attackDefinition.damage;
        }

        public int getKnockbackForce()
        {
            return baseEnemy.attackDefinition.knockback;
        }
    }
}