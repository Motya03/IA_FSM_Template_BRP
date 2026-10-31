using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
        //AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        //if (!stateInfo.IsName("Firing Rifle"))
        //{
        //    anim.SetTrigger("isShooting");
        //}

      
        base.Enter();
    }
	
    public override void Update()
    {
        
        bool playerInSight = CanSeePlayer();
        bool canAtack = CanAttackPlayer();
       // Debug.Log(canAtack);
       // Debug.Log(playerInSight);
        if (playerInSight && !canAtack)
        {
           // anim.SetTrigger("isRunning");
            nextState = new Pursue(npc, agent, anim, player);
            stage = EVENT.EXIT;
           
        }

        if (playerInSight && canAtack)
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (!stateInfo.IsName("Firing Rifle"))
            {
                anim.ResetTrigger("isRunning");
              //  anim.ResetTrigger("isIdle");
               // anim.ResetTrigger("isShooting");
                anim.SetTrigger("isShooting");
                Debug.Log("Shooting");
            }

            agent.isStopped = true;
            //TODO
            //El NPC debe girar hacia el jugador y dispararle.
            Vector3 direction = player.position - npc.transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
           
        }


        if (!playerInSight && !canAtack)
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
