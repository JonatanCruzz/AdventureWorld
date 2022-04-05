using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;
using UnityEngine.AI;

public class AdvancedEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    private NavMeshAgent agent;
    private Player player;
    AnimancerComponent animancer;

    public AnimationClip idle;
    public AnimationClip walk;
    public AnimationClip attack;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        var playerPos = player.transform.position;
        var pos = transform.position;
        var distance = Vector3.Distance(playerPos, pos);

        if (distance < 10)
        {
            agent.SetDestination(playerPos);
            // animancer.Play(walk);
        
            if (distance < 2)
            {
                // animancer.Play(attack);
            }
        }
        else
        {
            // animancer.Play(idle);
        }
    }
}