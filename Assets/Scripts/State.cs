using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State
{
    public enum STATE
    {
        IDLE, PATROL, PURSUE, ATTACK, SLEEP, RUNAWAY
    };
    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    };
    public STATE name; //Basado en el anteior Enum, guardara el estado
    protected EVENT stage; //Sera Enter Update o Exit
    protected GameObject npc;
    protected Animator anim; //Tratamos con animaciones
    protected Transform player; //El guardian necesita saber donde esta el jugador
    protected State nextState; //Estado siguiente al que estamos actualmente
    protected NavMeshAgent agent;
    public float visionDistance = 10f;
    public float angleVision = 60f;
    

    //When the NPC is able to see or interact with the player
    float visDist = 10.0f;
    float visAngle = 30.0f; //En grados
    float shootDist = 7.0f; //Ataca si esta asi de cerca

    //Constructor
    public State(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
    {
        npc = _npc;
        agent = _agent;
        anim = _anim;
        stage = EVENT.ENTER;
        player = _player;
    }
    //Enter es el primer evento del estado y directamente nos lleva a Update
    public virtual void Enter()  { stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT;  }

    //Parte mas importante del codigo de estados, loop principal
    public State Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState; //Siguiente estado
        }
        return this; //Devolvemos el mismo estado
    }

    //Estas funciones deberian estar en otro fichero CS, pero para poder
    //seguir mejor el codigo lo pondremos todo aqui
    public bool CanSeePlayer()
    {
        Vector3 direccion = player.position - npc.transform.position;

        // 1. Comprobar distancia
        if (direccion.magnitude > visionDistance)
            return false;

        // 2. Comprobar ángulo de visión
        float angulo = Vector3.Angle(npc.transform.forward, direccion);

        if (angulo > angleVision / 2f)
            return false;

        // 3. Comprobar si hay obstáculos
        if (Physics.Raycast(
            npc.transform.position,
            direccion.normalized,
            out RaycastHit hit,
            visionDistance))
        {
            return hit.transform == player ||
                   hit.transform.IsChildOf(player);
        }

        return false;
        
    }


    

//El jugador esta detras del NPC a una distancia menor a 2 y en un angulo de 30
public bool IsPlayerBehind()
    {
        //TODO  
        return false;
    }

    public bool CanAttackPlayer()
    {
        //TODO  
        return false;
    }
}