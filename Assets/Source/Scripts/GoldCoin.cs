using System;
using System.Collections;
using System.Collections.Generic;
using AdventureWorld.Prueba;
using UnityEngine;

public class GoldCoin : MonoBehaviour
{
    public float range = 3;
    public float pickupRange = 1;
    public int value = 1;
    private Player _player;

    public Player player
    {
        get => _player;
        set { _player = value; }
    }

    public void Start()
    {
        Utils.CreateTrigger(gameObject, range, Vector2.zero, new[] {typeof(CoinTrigger)});
    }

    public void Update()
    {
        // follow the player. 
        if (player == null) return;

        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, 0.1f );
        // if the player is close enough, pick it up. 
        var distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance < pickupRange)
        {
            player.AddGold(this);
            Destroy(gameObject);
        }
        Debug.Log("distance: " + distance);
        Debug.DrawLine(transform.position, player.transform.position, Color.red);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}

public class CoinTrigger : MonoBehaviour
{
    private GoldCoin _coin;

    private void Start()
    {
        _coin = GetComponentInParent<GoldCoin>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        var player = other.GetComponent<Player>();
        if (player == null) return;

        _coin.player = player;
    }
}