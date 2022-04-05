using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveBehaviour : MonoBehaviour
{
    public Vector2 moveDirection = Vector2.zero;
    private Vector2 p_moveDirection = Vector2.zero;
    public float moveSpeed = 5;
    public float dashFactor = 2;
    public float Dash_T = 0;
    public float DashTime = 1f;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private HealthUnit _health;
    public bool movePrevent;
    public bool disableAnimation = false;
    public bool Dashing
    {
        get => Dash_T > 0;
    }

    public AttackSpecifications Attack;

    public void Move(InputAction.CallbackContext ctx)
    {
        if (this.Dashing)
        {
            p_moveDirection = ctx.ReadValue<Vector2>();
            return;
        }

        this.moveDirection = ctx.ReadValue<Vector2>();

        // UnityEngine.Input.ac
    }

    public void Dash()
    {
        if (this.moveDirection == Vector2.zero) return;
        p_moveDirection = this.moveDirection;
        this.Dash_T = this.DashTime;
    }

    public void StopDash()
    {
        this.Dash_T = 0;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _health = GetComponent<HealthUnit>();
    }

    private bool isAlive()
    {
        return _health == null || _health.HP > 0;
    }

    private void applyAnimation(Vector2 direction)
    {
        if (disableAnimation || this._animator == null) return;
        _animator.SetFloat("movX", direction.x);
        _animator.SetFloat("movY", direction.y);
    }

    private void FixedUpdate()
    {
        if (!isAlive()) return;
        if (this.movePrevent) return;
        if (this.Dashing)
        {
            this.Dash_T -= Time.fixedDeltaTime;
            if (!this.Dashing)
            {
                this.moveDirection = p_moveDirection;
            }

            _rigidbody.velocity = this.moveDirection * this.moveSpeed * this.dashFactor;
            return;
        }

        if (this.Attack.attackDirection != Vector2.zero)
        {
            this.applyAnimation(this.Attack.attackDirection);
            PhysicsUtils.DoMoveRigidBodyByKnockback(_rigidbody, this.Attack.attackDirection, this.Attack.knockback,
                Time.fixedDeltaTime);
            return;
        }

        _rigidbody.MovePosition(_rigidbody.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        this.applyAnimation(moveDirection);
    }
}