using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //Libreria para la ordenacion

//Esta clase no es MonoBehavior, sino a public sealed para nuestra lista de check points
public sealed class GameEnvironment
{
    private static GameEnvironment instance;
    //Lista privada de GameObjects
    private List<GameObject> checkpoints = new List<GameObject>();
    //Lista para manejar la lista privada
    public List<GameObject> Checkpoints { get { return checkpoints; } }

    private GameObject[] hidingSpots;

    public static GameEnvironment Singleton
    {
        get
        {
            if (instance == null)
            {
                instance = new GameEnvironment();
                instance.Checkpoints.AddRange(GameObject.FindGameObjectsWithTag("Checkpoint"));
                //Solo con "FindGameObjectsWithTag" no aseguramos que el orden de puntos sea el que queremos
                //Hacemos que los puntos esten ordenados por orden alfabetico para que la ruta del personaje sea
                //como la hemos puesto nosotros (en cuadro). En la Hierarchy les pondremos el nombre (sabiendo que
                //se ordenan alfabeticamente) para que siga la ruta que queremos
                instance.checkpoints = instance.checkpoints.OrderBy(waypoint => waypoint.name).ToList();

               instance.hidingSpots = GameObject.FindGameObjectsWithTag("Safe");
            }
            return instance;
        }
    }

    public GameObject[] GetHidingSpots()
    {
        return hidingSpots;
    }
}
