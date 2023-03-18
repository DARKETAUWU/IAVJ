using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ASunAgent : NewSteeringBehavior
{
    public Vector3 v3StartPosition;
    public Vector3 v3EndPosition;

    public GameObject GOStart, GOEnd;

    // Booleano que marca si el agente es seleccionado o no
    public bool bSelected = false;

    // Lista donde guardaremos los puntos que nos regrese el m�todo de A*
    public List<Vector3> Route;

    // Qu� tan cerca tiene que estar el agente del punto objetivo para cambiar al siguiente punto.
    public float fDistanceThreshold;

    PathFinding _PathfindingReference;

    ClassGrid _GridReference;

    int iCurrentRoutePoint = 0;

    void Start()
    {
        _PathfindingReference = GameObject.FindGameObjectWithTag("grid").GetComponent<PathFinding>();

        _GridReference = _PathfindingReference.myGrid;

        // Guardamos el resultado de nuestro A*
        List<Node> AStarResult = _GridReference.AStarSearch(0, 0, 4, 0);

        // Usamos esa lista de nodos para sacar las posiciones de mundo a las cuales hacer Seek o Arrive.
        Route = _GridReference.ConvertBacktrackToWorldPos(AStarResult);
    }

    void Update()
    {
        //Si esta marcado y se usa click izquierdo:
        if (bSelected && Input.GetKey(KeyCode.Mouse0))
        {
            Vector3 mouse0Pos = Input.mousePosition/10;
            {
                v3StartPosition = mouse0Pos;
                GOStart.transform.position = mouse0Pos;
            }
        }

        //Si esta marcado y se usa click derecho:
        if (bSelected && Input.GetKey(KeyCode.Mouse1))
        {
            Vector3 mouse1Pos = Input.mousePosition/10;
            {
                v3EndPosition = mouse1Pos;
                GOEnd.transform.position = mouse1Pos;
            }
        }

        ////Si esta marcado y se presiona la rueda del rat�n:
        //if (bSelected && Input.GetKey(KeyCode.Mouse2))
        //{
        //    Vector3 mouse2Pos = Input.mousePosition;
        //    {
        //tengo pensado que con este se marque como no caminable
        //    }
        //}

    }

    private void FixedUpdate()
    {
        Vector3 v3SteeringForce = Vector3.zero;

        if (Route != null)
        {
            // Queremos saber si estamos cerca de nuestra posici�n objetivo.
            // Para ello, calculamos la distancia entre la posici�n Route[iCurrentRoutePoint] y la actual.
            float fDistanceToPoint = (Route[iCurrentRoutePoint] - transform.position).magnitude;
            Debug.Log("fDistance to Point is: " + fDistanceToPoint);

            // Si esta distancia es menor o igual a un umbral, cambiamos al siguiente punto de la lista.
            if (fDistanceToPoint < fDistanceThreshold)
            {
                iCurrentRoutePoint++;
                iCurrentRoutePoint = math.min(iCurrentRoutePoint, Route.Count - 1);
                // Hay que checar si es el �ltimo punto de la ruta a seguir.
                //if (iCurrentRoutePoint >= Route.Count)
                //{
                //    // Ya nos acabamos los puntos de esta ruta.
                //    // Podemos borrar esta ruta que terminamos.
                //    Route = null;
                //    iCurrentRoutePoint = -1;
                //    bUseArrive = false;
                //    return;
                //}
            }

            if (iCurrentRoutePoint == Route.Count - 1)
            {
                bUseArrive = true;
                v3SteeringForce = Seek(Route[iCurrentRoutePoint]);
            }
            else
            {
                // Ahora s�, hay que mover el agente hacia el punto destino.
                v3SteeringForce = Seek(Route[iCurrentRoutePoint]);
            }
        }

        // Idealmente, usar�amos el ForceMode de Force, para tomar en cuenta la masa del objeto.
        // Aqu� ya no usamos el deltaTime porque viene integrado en c�mo funciona AddForce.
        myRigidbody.AddForce(v3SteeringForce /* Time.deltaTime*/, ForceMode.Acceleration);

        // Hacemos un Clamp para que no exceda la velocidad m�xima que puede tener el agente
        myRigidbody.velocity = Vector3.ClampMagnitude(myRigidbody.velocity, fMaxSpeed);
    }
    public void Selected()
    {
        if (bSelected == true)
        {
            bSelected = false;
            return;
        }
        
        else if(bSelected == false)
        {
            bSelected = true;
            return;
        }
    }
}
/*Requisitos del programa:
1) Control del agente y de la aplicaci�n: (Total: 60 puntos).

c. Una vez seleccionado, con el click derecho del mouse en la cuadr�cula, seleccionar el
cuadro/nodo fin para el pathfinding, excepto si es click sobre el AStarAgent
seleccionado, en cuyo caso, se deseleccionar�. 10 puntos.


c. Una vez seleccionado, con el click derecho del mouse en la cuadr�cula, seleccionar el
cuadro/nodo fin para el pathfinding, excepto si es click sobre el AStarAgent
seleccionado, en cuyo caso, se deseleccionar�. 10 puntos.

d. Si se selecciona el nodo inicio para pathfinding como el nodo fin para pathfinding (o
viceversa), se debe sobrescribir y ser el nuevo nodo fin/inicio, respectivamente.
e. Una vez que haya un nodo inicio y un nodo fin de pathfinding, al presionar un bot�n
(por ejemplo, click central o barra espaciadora), ejecutar el pathfinding y que su

AStarAgent se mueva por el camino usando steering behaviors como se vi� en la clase
del 9 de marzo de 2023. 30 puntos.
f. NOTA: En clase vimos c�mo convertir el X y Y de un nodo a su posici�n de mundo; para
hacer esta funci�n tienen que hacer el proceso �inverso�, es decir, convertir de una
posici�n de mundo (o pantalla, seg�n sea su implementaci�n) a su posici�n en
coordenadas X y Y de la cuadr�cula.

2) Mostrar los valores g_cost, h_cost, y f_cost de la cuadr�cula (tras ejecutar A*). (Total 15 puntos).

3) Cambiar el color del texto de cada nodo seg�n si est� en la lista abierta (verde), cerrada (azul), o
la ruta encontrada (Rojo). (Total 25 puntos).
a. Poner el color de fondo de la escena casi blanco o casi negro para que se vea
adecuadamente los colores del pathfinding.
b. Este cambio de color debe permanecer en pantalla hasta que se inicie otro pathfinding
distinto.
c. NOTA: Esto requiere que piensen bien d�nde deben declararse las listas abierta y
cerrada, as� como en d�nde se inicializan.
4) Puntos extra: (hasta 65 puntos obtenibles)
a. Poder poner nodos no-caminables para el pathfinding.
i. Usar alg�n bot�n o tecla para que, al tener el mouse sobre un cuadro/nodo,
�ste cambie entre caminable y no-caminable (al presionar se vuelve lo contrario
de lo que estaba). 20 puntos.
ii. Durante el pathfinding, los nodos no-caminables deben meterse en la lista
cerrada en cuanto se encuentran. 5 puntos.
iii. Hacer una funci�n que guarde el estado actual del grid (cuadr�cula,
cu�les son caminables y cu�les no, as� como los costos), y poder cargar los grids.
15 puntos.

b. Poder tener m�s de un AStarAgent en la escena y poder usarlos correctamente como si
solo hubiese uno. Solo elegir uno de los dos siguientes.
i. En este caso, la cuadr�cula debe mostrar los valores/colores y ruta del agente
que se haya seleccionado y ejecutado su pathfinding al �ltimo. 15 puntos.
ii. Si quieren un reto: Que cada AStarAgent tenga su propia cuadr�cula VISUAL que
se pueda activar y desactivar. 25 puntos.
1. NOTA: Esto requiere que ciertos objetos se dupliquen, la clave est� en
que piensen bien cu�les.*/
