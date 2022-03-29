using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class MoveBehaviour : MonoBehaviour
{
    public Vector2 moveDirection = Vector2.zero;
    public float moveSpeed = 5;

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private HealthUnit _health;

    public AttackSpecifications Attack;
    public void Move(InputAction.CallbackContext ctx)
    {
        this.moveDirection = ctx.ReadValue<Vector2>();
        // UnityEngine.Input.ac
    }
    public void Move2()
    {
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
    private void FixedUpdate()
    {
        if (!isAlive()) return;
        if (this.Attack.attackDirection != Vector2.zero)
        {
            _animator.SetFloat("movX", this.Attack.attackDirection.x);
            _animator.SetFloat("movY", this.Attack.attackDirection.y);
            PhysicsUtils.DoMoveRigidBodyByKnockback(_rigidbody, this.Attack.attackDirection, this.Attack.knockback, Time.fixedDeltaTime);
            return;
        }
        _rigidbody.MovePosition(_rigidbody.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        _animator.SetFloat("movX", moveDirection.x);
        _animator.SetFloat("movY", moveDirection.y);
    }

}
