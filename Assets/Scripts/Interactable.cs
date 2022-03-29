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

    // this method will be overridden by other classes
    public abstract void OnClick();

    public abstract bool isClicked();
    private bool _isClicked;
    // determine if the player is inside the collider
    public bool IsCloseEnough()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var playerCollider = player.GetComponent<Collider2D>();

        return ((Vector2)gameObject.transform.position - (Vector2)playerCollider.transform.position).sqrMagnitude < radius * radius;
    }
    public virtual bool IsInteracting()
    {
        return _isClicked;
    }

    private IEnumerator _onClick()
    {
        OnClick();
        _isClicked = true;
        yield return new WaitForSeconds(1);
        _isClicked = false;
    }

    public void Update()
    {
        if (!_isClicked && !IsInteracting() && IsCloseEnough())
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