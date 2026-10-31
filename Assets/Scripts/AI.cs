using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

//Este es el codigo que adjuntaremos a nuestro NPC (Ugg)
public class AI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator anim;
    [SerializeField] private Transform player;
    public int index ;

    State currentState;
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();
        currentState = new Idle(this.gameObject, agent, anim, player);
    }

	void Update()
    {
       
        currentState = currentState.Process();
    }
   public void PlusIndex()
    {
                index++;
        if (index >= 5)
        {
            index = 0;
        }
    }
    public void PlayShootSound()
    {
        AudioSource shoot = this.GetComponent<AudioSource>();
        shoot.PlayOneShot(shoot.clip);
    }
}
