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
    public Collider2D _collider;

    // this method will be overridden by other classes
    public abstract void OnClick();

    public abstract bool isClicked();
    private bool _isClicked;
    // determine if the player is inside the collider
    public bool IsCloseEnough()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var playerCollider = player.GetComponent<Collider2D>();
        return _collider.bounds.Contains(playerCollider.bounds.center);
    }

    public void Update()
    {
        if (!_isClicked && IsCloseEnough())
        {
            uiKey.SetActive(true);
            if (isClicked())
            {
                OnClick();
                this._isClicked = true;
                Task.Delay(TimeSpan.FromSeconds(1)).ContinueWith(t => this._isClicked = false);
            }
        }
        else
        {
            uiKey.SetActive(false);
        }
    }
}