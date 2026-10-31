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
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (!stateInfo.IsName("Rifle Run"))
        {
            // anim.ResetTrigger("isRunning");
            // anim.ResetTrigger("isIdle");
            anim.ResetTrigger("isWalking");
            anim.ResetTrigger("isShooting");
            anim.SetTrigger("isRunning");
            Debug.Log("Pursue");
        }

       
        agent.isStopped = false;
        agent.speed = 3f;
        agent.SetDestination(player.position);
        
        base.Enter();
    }
    public override void Update()
    {
       
        bool playerInSight = CanSeePlayer();
        bool canAtack = CanAttackPlayer();
       if (canAtack )
        {
            agent.isStopped = true;
            nextState = new Attack(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
       if (!playerInSight)
        {
          nextState = new Patrol(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
      

    }

    public override void Exit()
    {
       
        base.Exit();
    }

}
