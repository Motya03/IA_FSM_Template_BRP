using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

//Mover el personaje por los wayPoints, necesita un index para saber por donde vamos
//Recuerda que no tenemos que guardar los waypoints en ningún sitio
//puesto que la clase GameEnvirontment (Singleton) ya lo hace
public class Patrol: State
{
   [SerializeField] private GameEnvironment game;
    public List<Transform> check = new List<Transform>();
    GameObject npcE1;
    NavMeshAgent agentE1;
    Animator animE1;
    Transform playerE1;
    int completedLoops;
    int currentIndex = -1;
    public Patrol(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
    {
        this.npc = npcE1;
        this.agent = agentE1;
        this.anim = animE1;
        this.player = playerE1;
        name = STATE.PATROL;
       //TODO
    }

    public override void Enter()
    {
        

        GetDestinatiion();
        stage = EVENT.UPDATE;
        Debug.Log("Start PAtrol");
        //TODO
        base.Enter();

    }
    public override void Update()
    {
    
    
        if (agentE1.remainingDistance == 0.1 )
        {
            GetDestinatiion();
            return;
        }
        if (player)
         //TODO
         stage = EVENT.EXIT;
    }

    public override void Exit()
    {
        //TODO
        base.Exit();
    }

    public Transform GetRandomTransform()
    {

       // GameObject[] check = game.Checkpoints
        // Проверяем, что список не пустой, чтобы избегнуть ошибок
        if (check == null || check.Count == 0)
        {
            Debug.LogWarning("Список Transform пуст!");
            return null;
        }

        // Получаем случайный индекс от 0 до количества элементов в списке
        int randomIndex = Random.Range(0, check.Count);

        // Возвращаем случайный Transform
        return check[randomIndex];
    }
    private void GetDestinatiion()
    {
        Transform targetPoint = GetRandomTransform();
        Vector3 targetPosition = targetPoint.position;
    }
}