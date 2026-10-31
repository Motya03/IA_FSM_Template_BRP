using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class RunAway : State
{
    //Si definimos como global "safeLocation" podremos acceder desde toda la clase
    GameObject safeLocation;
    public RunAway(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
    {
        name = STATE.RUNAWAY;
        safeLocation = GameObject.FindGameObjectWithTag("Safe");
    }
    public override void Enter()
    {
        agent.isStopped = false;
        agent.SetDestination(safeLocation.transform.position);
        agent.speed = 3f;
        anim.ResetTrigger("isShooting");
        anim.ResetTrigger("isWalking");
       if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Rifle Run"))
        {
            anim.SetTrigger("isRunning");
        }
      
        base.Enter();
    }
    public override void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            nextState = new Patrol(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
        //TODO
        
    }

    public override void Exit()
    {
        //TODO
        base.Exit();
    }

}
