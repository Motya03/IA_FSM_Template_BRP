using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Pursue: State
{
    private float stopDistance = 5f;
    public Pursue(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
    {
        name = STATE.PURSUE;
        
    }
    public override void Enter()
    {
        agent.SetDestination(player.position);
        stage = EVENT.UPDATE;
        base.Enter();
    }
    public override void Update()
    {
       if (agent.remainingDistance <= stopDistance)
        {
            agent.isStopped = true;
            stage = EVENT.EXIT;
        }
        
    }

    public override void Exit()
    {
        nextState = new Attack(npc, agent, anim, player);
        base.Exit();
    }

}
