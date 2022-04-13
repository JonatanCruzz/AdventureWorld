using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

// create a class that defines a GameObject that can be interacted with
// this class will be inherited by other classes
// An interactable Object should have a radius to be able to interact with it
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

    private bool canShowKey = false;
    private Coroutine listenKeyCoroutine;
    // this method will be overridden by other classes
    protected abstract void OnClick(Player player);

    private bool _isClicked;
    public void Start()
    {
        // create a child gameobject with a trigger collider with the radius of the interactable object
        // this will be used to detect if the player is within the radius of the interactable object
        // it should have a script attached to it that will call the OnTriggerEnter2D method
        var trigger = new GameObject("Trigger", new []{typeof(CircleCollider2D), typeof(InteractableTrigger)})
        {
            transform =
            {
                parent = this.transform,
                localPosition = new Vector3(0, 0, 0)
            }
        };
        var triggerCollider = trigger.GetComponent<CircleCollider2D>();
        triggerCollider.radius = this.radius;
        triggerCollider.isTrigger = true;
        triggerCollider.offset = this.offset;
    }
    // determine if the player is inside the collider

    public virtual bool IsInteracting()
    {
        return _isClicked;
    }

    private bool canInteract()
    {
        return !_isClicked && canShowKey && !IsInteracting();
    }


    public void ShowKey(Player p)
    {
       
        canShowKey = true;

    }

    public void HideKey()
    {
        canShowKey = false;
       
    }

    private IEnumerator _doClick(Player p_Player)
    {
        OnClick(p_Player);
        _isClicked = true;
        yield return new WaitForSeconds(1);
        _isClicked = false; 
    }

    public void DoClick(Player p_Player)
    {
        StartCoroutine(_doClick(p_Player));
    }

    public void Update()
    {
        uiKey.SetActive(canInteract());
    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere((Vector2)transform.position + offset, radius);

    }
}