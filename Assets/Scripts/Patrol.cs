using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



//Mover el personaje por los wayPoints, necesita un index para saber por donde vamos
//Recuerda que no tenemos que guardar los waypoints en ningún sitio
//puesto que la clase GameEnvirontment (Singleton) ya lo hace
public class Patrol: State
{
  
    private List<GameObject> checkpoints;
    public bool notOnTheWay;
    float canCheckTime = 0.5f;
    float timer;
    int indexCount;



    int completedLoops;
    
    Vector3 targetNpc;
  
    private int currentCheckpoint = 0;
   
    public Patrol(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
    {
        
        name = STATE.PATROL;

       //TODO
    }

    public override void Enter()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (!stateInfo.IsName("Rifle Walk"))
        {
            anim.ResetTrigger("isRunning");
          //  anim.ResetTrigger("isIdle");
            anim.ResetTrigger("isShooting");
            anim.SetTrigger("isWalking");
            Debug.Log("Walk");
        }
       
        AI ia = npc.GetComponent<AI>();

        indexCount = ia.index;
        timer = 0;
        checkpoints = GameEnvironment.Singleton.Checkpoints;

        // Avanzar al siguiente checkpoint
        currentCheckpoint = (currentCheckpoint + indexCount) % checkpoints.Count;

       
        GetDestination();
        agent.speed = 1f;
        agent.isStopped = false;
       // anim.SetTrigger("isWalking");

        

      
        agent.SetDestination(targetNpc);
       ia.PlusIndex();
    
        base.Enter();
    }
    public override void Update()
    {

      
        bool playerInSight = CanSeePlayer();
        bool playerBehind = IsPlayerBehind();

        // Comprobar si hemos llegado al destino
        if (HasReachedDestination())
        {
           
            nextState = new Idle(npc, agent, anim, player);
            stage = EVENT.EXIT;
            return;
        }

        if (playerInSight)
        {
           
            anim.SetTrigger("isRunning");
            nextState = new Pursue(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
        if (playerBehind)
        {
            
            nextState = new RunAway(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
    }

    // Método auxiliar para comprobar si llegamos al destino de forma segura
    private bool HasReachedDestination()
    {
        // Comprobaciones de seguridad
        if (agent.pathPending) return false;
        if (!agent.hasPath) return false;
        if (agent.pathStatus != NavMeshPathStatus.PathComplete) return false;

        // Comprobar distancia con un pequeño margen
        return agent.remainingDistance <= agent.stoppingDistance + 0.1f;
    }
    public override void Exit()
    {
       // NextCheckpoint();
        //TODO
        base.Exit();
    }

   
    private void GetDestination()
    {
        checkpoints = GameEnvironment.Singleton.Checkpoints;

        targetNpc = checkpoints[currentCheckpoint].transform.position;

    }

   
    
}