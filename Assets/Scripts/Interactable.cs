using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

// create a class that defines a GameObject that can be interacted with
// this class will be inherited by other classes
// An interactable Object should have a collider and a script attached to it
// An interactable Object should have a key to interact with it
// An interactable Object should have a description of what it does
// An interactable Object should have a method to execute when interacted with
[RequireComponent(typeof(Collider2D))]
public abstract class Interactable : MonoBehaviour
{
    public GameObject uiKey;

    public string description;
    public float radius = 1.5f;
    public Vector2 offset = new Vector2(0, 0f);
    private Player p_Player;
    private UnityEngine.InputSystem.InputAction action;
    // this method will be overridden by other classes
    public abstract void OnClick(Player player);

    public abstract bool isClicked();
    private bool _isClicked;
    public void Start()
    {
        this.p_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    // determine if the player is inside the collider
    public bool IsCloseEnough()
    {

        return Vector2.Distance(transform.position, p_Player.transform.position) < radius;
    }
    public virtual bool IsInteracting()
    {
        return _isClicked;
    }

    private IEnumerator _onClick()
    {
        OnClick(p_Player);
        _isClicked = true;
        yield return new WaitForSeconds(1);
        _isClicked = false;
    }

    public void Update()
    {
        if (!_isClicked && p_Player.canInteract && !IsInteracting() && IsCloseEnough())
        {
            uiKey.SetActive(true);
            if (isClicked())
            {
                StartCoroutine(_onClick());
            }
        }
        else
        {
            uiKey.SetActive(false);
        }
    }

    // Podemos dibujar el radio de visión y ataque sobre la escena dibujando una esfera
    void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere((Vector2)transform.position + offset, radius);

    }
}