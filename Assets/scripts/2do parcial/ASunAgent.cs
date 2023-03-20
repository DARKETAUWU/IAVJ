using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ASunAgent : NewSteeringBehavior
{
    public Vector3 v3StartPosition;
    public Vector3 v3EndPosition;

    public Collider CCollider;
    public GameObject GOStart, GOEnd;

    // Lista donde guardaremos los puntos que nos regrese el método de A*
    public List<Vector3> Route;

    // Qué tan cerca tiene que estar el agente del punto objetivo para cambiar al siguiente punto.
    public float fDistanceThreshold;

    PathFinding _PathfindingReference;

    ClassGrid _GridReference;

    
    

    int iCurrentRoutePoint = 0;

    
    void Start()
    {
        myRigidbody.isKinematic = true; //se pone rigidbody true para que no se mueva

        _PathfindingReference = GameObject.FindGameObjectWithTag("grid").GetComponent<PathFinding>();

        _GridReference = _PathfindingReference.myGrid;
    }

    //funciona para moverse en linea recta y buscando el final del nodo

    public void StartRoute()
    {
        //imprime cual es el nodo final y sus cordenadas
        Debug.Log("El nodo Fin es " + EndNode._Xend + ", " + EndNode._Yend);
        //imrpime el nodo inicio y sus cordenadas
        Debug.Log("El nodo inicio es " + StartNode._Xstart + "," + StartNode._Ystart);
        transform.position = GOStart.transform.position;
        // Guardamos el resultado de nuestro A* lo cual hace que sea su inicio y su meta
        List<Node> AStarResult = _GridReference.AStarSearch(StartNode._Xstart, StartNode._Ystart, EndNode._Xend, EndNode._Yend);
        // Usamos esa lista de nodos para sacar las posiciones de mundo a las cuales hacer Seek o Arrive.
        Route = _GridReference.ConvertBacktrackToWorldPos(AStarResult);

        //Manda al monito a la posicion inicial del nodo
        CCollider.isTrigger = true;
        myRigidbody.isKinematic = false;
        //Route[0] = GOStart.transform.position;
        //Route[1] = GOEnd.transform.position;
    }

    private void FixedUpdate()
    {
        Vector3 v3SteeringForce = Vector3.zero;

        if (Route != null)
        {
            // Queremos saber si estamos cerca de nuestra posición objetivo.
            // Para ello, calculamos la distancia entre la posición Route[iCurrentRoutePoint] y la actual.
            float fDistanceToPoint = (Route[iCurrentRoutePoint] - transform.position).magnitude;
            //Debug.Log("fDistance to Point is: " + fDistanceToPoint);

            // Si esta distancia es menor o igual a un umbral, cambiamos al siguiente punto de la lista.
            if (fDistanceToPoint < fDistanceThreshold)
            {
                iCurrentRoutePoint++;
                iCurrentRoutePoint = math.min(iCurrentRoutePoint, Route.Count - 1);

            }

            if (iCurrentRoutePoint == Route.Count - 1)
            {
                bUseArrive = true;
                v3SteeringForce = Seek(Route[iCurrentRoutePoint]);
            }
            else
            {
                // Ahora sí, hay que mover el agente hacia el punto destino.
                v3SteeringForce = Seek(Route[iCurrentRoutePoint]);
            }
        }

        //Se le implementa la fuerza de movimiento al monito
        myRigidbody.AddForce(v3SteeringForce, ForceMode.Acceleration);

        // Se le implementa un clamp para que no exeda la velocidad maxima
        myRigidbody.velocity = Vector3.ClampMagnitude(myRigidbody.velocity, fMaxSpeed);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Default"))
        {
            if (StartEnd.bA2 == true)
                StartEnd.bA2 = false;

            if (StartEnd.bA1 == false)
            {
                StartEnd.bA1 = true; //se verifica si es el agente 1 esta seleccionado
                Debug.Log("VERDAD1");
                return;
            }

            else
            {
                Debug.Log("FALSO1");
                StartEnd.bA1 = false; //se verica siel agente 2 fue deseleccionado
            }

        }


    }

}

