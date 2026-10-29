using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Idle : State //Clase Idle que hereda de State
{
    GameObject npcE1;
    NavMeshAgent agentE1;
    Animator animE1;
    Transform playerE1;
    float idleTime = 2f;
    float timer;
    public Idle(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
    {
        this.npc = npcE1;
        this.agent = agentE1;
        this.anim = animE1;
        this.player = playerE1;
        name = STATE.IDLE;
    }

    //Debemos ahora crear las 3 funciones (public override) para las etapas de Enter, Update y Exit de esta clase/estado
    public override void Enter()
    {
        //TODO
        timer = 0;
        stage = EVENT.UPDATE;
        Debug.Log("Entering Idle");
        base.Enter(); //Aqui ya se hace stage = EVENT.UPDATE;, no necesitamos hacerlo aqui
    }

    //Update se ejecuta una y otra vez y necesita un evento que le haga salir (que rompa la secuencia)
    public override void Update()
    {
        timer += Time.deltaTime;
        if (timer > idleTime)
        {
            nextState = new Patrol(npcE1, agentE1, animE1, playerE1);
            stage = EVENT.EXIT;
        }
        
    }
    public override void Exit()
    {
        //TODO
        Debug.Log("Exititing Idle");
        base.Exit();
    }
}
