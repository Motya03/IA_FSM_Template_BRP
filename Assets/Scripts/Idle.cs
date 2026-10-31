using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Idle : State //Clase Idle que hereda de State
{
    //GameObject npcE1;
    //NavMeshAgent agentE1;
    //[SerializeField] private Animator animE1;
    //Transform playerE1;
    float idleTime = 5f;
    float timer;
    public Idle(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
    {
        //this.npc = npcE1;
        //this.agent = agentE1;
        //this.anim = animE1;
        //this.player = playerE1;
        name = STATE.IDLE;
    }

    //Debemos ahora crear las 3 funciones (public override) para las etapas de Enter, Update y Exit de esta clase/estado
    public override void Enter()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (!stateInfo.IsName("Rifle Idle"))
        {
            anim.SetTrigger("isIdle");
        }
      
       
        //TODO
        timer = 0;
        
        agent.isStopped = true;
        // stage = EVENT.UPDATE;
        
       
        base.Enter(); //Aqui ya se hace stage = EVENT.UPDATE;, no necesitamos hacerlo aqui
    }

    //Update se ejecuta una y otra vez y necesita un evento que le haga salir (que rompa la secuencia)
    public override void Update()
    {
       
        timer += Time.deltaTime;
        bool playerInSight = CanSeePlayer();
        bool playerBehind = IsPlayerBehind();
        if (playerInSight)
        {
            nextState = new Pursue(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
        if (playerBehind)
        {

            nextState = new RunAway(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
        if (timer > idleTime)
        {
           
            nextState = new Patrol(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
        
    }
    public override void Exit()
    {
        //TODO
      
        base.Exit();
    }
}
