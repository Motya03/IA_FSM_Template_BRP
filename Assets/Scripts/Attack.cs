using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Attack : State
{
	//El NPC debe seguir al jugador y girar mirándolo.
    float rotationSpeed = 2.0f;
    AudioSource shoot;
    public Attack(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
    {
        name = STATE.ATTACK;
        //Fuente de audio que lleva adjunta el NPC (Ugg)
        shoot = _npc.GetComponent<AudioSource>();
		//Podriamos hacerlo en la clase State en vez de cargarlo cada vez
        //que se ataca, pero aqui lo hemos hecho asi.
    }
	
    public override void Enter()
    {
        stage = EVENT.UPDATE;
        base.Enter();
    }
	
    public override void Update()
    {
        //TODO
        stage = EVENT.EXIT;
    }
    public override void Exit()
    {
        //TODO
        base.Exit();
    }
}
