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


        //List<Node> AStarResult = _GridReference.AStarSearch(0, 1, 4, 1);
        //se modificara la barra para que podamos ingresarle al momento de chocar las bases mmmm
        //puedo.... hacer que cuando detecte el grid le mande un valor primero y despues el otro
        //creo queserian muchos ifs
        //o un case
    }

    //funciona para moverse en linea recta y buscando el final del nodo

    public void StartRoute()
    {
        Debug.Log("El nodo Fin es " + EndNode._Xend + ", " + EndNode._Yend);
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

        // Idealmente, usaríamos el ForceMode de Force, para tomar en cuenta la masa del objeto.
        // Aquí ya no usamos el deltaTime porque viene integrado en cómo funciona AddForce.
        myRigidbody.AddForce(v3SteeringForce /* Time.deltaTime*/, ForceMode.Acceleration);

        // Hacemos un Clamp para que no exceda la velocidad máxima que puede tener el agente
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
/*Requisitos del programa:

d. Si se selecciona el nodo inicio para pathfinding como el nodo fin para pathfinding (o
viceversa), se debe sobrescribir y ser el nuevo nodo fin/inicio, respectivamente.


2) Mostrar los valores g_cost, h_cost, y f_cost de la cuadrícula (tras ejecutar A*). (Total 15 puntos).

3) Cambiar el color del texto de cada nodo según si está en la lista abierta (verde), cerrada (azul), o
la ruta encontrada (Rojo). (Total 25 puntos).
a. Poner el color de fondo de la escena casi blanco o casi negro para que se vea
adecuadamente los colores del pathfinding.
b. Este cambio de color debe permanecer en pantalla hasta que se inicie otro pathfinding
distinto.
c. NOTA: Esto requiere que piensen bien dónde deben declararse las listas abierta y
cerrada, así como en dónde se inicializan.
4) Puntos extra: (hasta 65 puntos obtenibles)
a. Poder poner nodos no-caminables para el pathfinding.
i. Usar algún botón o tecla para que, al tener el mouse sobre un cuadro/nodo,
éste cambie entre caminable y no-caminable (al presionar se vuelve lo contrario
de lo que estaba). 20 puntos.
ii. Durante el pathfinding, los nodos no-caminables deben meterse en la lista
cerrada en cuanto se encuentran. 5 puntos.
iii. Hacer una función que guarde el estado actual del grid (cuadrícula,
cuáles son caminables y cuáles no, así como los costos), y poder cargar los grids.
15 puntos.

b. Poder tener más de un AStarAgent en la escena y poder usarlos correctamente como si
solo hubiese uno. Solo elegir uno de los dos siguientes.
i. En este caso, la cuadrícula debe mostrar los valores/colores y ruta del agente
que se haya seleccionado y ejecutado su pathfinding al último. 15 puntos.
ii. Si quieren un reto: Que cada AStarAgent tenga su propia cuadrícula VISUAL que
se pueda activar y desactivar. 25 puntos.
1. NOTA: Esto requiere que ciertos objetos se dupliquen, la clave está en
que piensen bien cuáles.*/
