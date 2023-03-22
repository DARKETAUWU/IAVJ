using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class Edge
//{
//    public Node A;
//    public Node B;
//    public float fCost;
//}

public class Node
{
    public int x;
    public int y;

    // public List<Node> Neighbors;
    public Node Parent;


    // Este es para a* y djikstra.
    public float f_Cost;  // El costo Final del nodo el cual es la suma del g_cost con h_cost
    public float g_Cost;  // el costo de haber llegado a la casilla
    public float h_Cost;  // El costo asociado a la casilla para poder pasar

    public float fTerrainCost;  // Costo del terreno (tipo monopoli) se tiene que pagar para poder pasar


    public bool bWalkable;  // Se puede caminar sobre este nodo o no.

    public Node(int in_x, int in_y)
    {
        this.x = in_x;
        this.y = in_y;
        this.Parent = null;
        this.g_Cost = int.MaxValue;
        this.f_Cost = int.MaxValue;
        this.h_Cost = int.MaxValue;
        this.fTerrainCost = 10;
        this.bWalkable = true;
    }

    public override string ToString()
    {
        return x.ToString() + ", " + y.ToString();
    }
}

public class ClassGrid
{
    public int iHeight; //se implementa la altura que dentra el tablero
    public int iWidth; // se implementa la anchura que dentra el tablero

    private float fTileSize; //tamaño de las casillas en las que podran pisar 
    private Vector3 v3OriginPosition; //Este vector funcionara para darle una posicion a las casillas

    public Node[,] Nodes; //array en el cual se implementaran los nodos
    public TextMesh[,] debugTextArray; //dentro de este array iremos implementando cada uno de los textos que necesitaremos

    public bool bShowDebug = true;
    public GameObject debugGO = null; //Sera el gameobject con la cual podremos ir añadiendo las casillas

    private GameObject GridGO; // se crean las casillas

   

    //clase para crear el grid, con la altura, la anchura el tamaño que tendran las casillas y la posicion que tomara dentro del mundo
    public ClassGrid(int in_height, int in_width, float in_fTileSize = 10.0f, Vector3 in_v3OriginPosition = default)
    {
        iHeight = in_height;
        iWidth = in_width;

        InitGrid();
        this.fTileSize = in_fTileSize;
        this.v3OriginPosition = in_v3OriginPosition;

        if (bShowDebug)
        {

            debugGO = new GameObject("GridDebugParent");

            debugTextArray = new TextMesh[iHeight, iWidth];

            for (int y = 0; y < iHeight; y++)
            {
                for (int x = 0; x < iWidth; x++)
                {
                    //crea el array
                    //funciona para escribir las posiciones del array
                    debugTextArray[y, x] =  CreateWorldText2(Nodes[y, x].ToString(),
                                            debugGO.transform, GetWorldPosition(x, y) + new Vector3(fTileSize * 0.5f, fTileSize * 0.5f),
                                            30, Color.green, TextAnchor.MiddleCenter);
                    
                    //Instantiate(tile, GetWorldPosition(x, y) + new Vector3(fTileSize * 0.5f, fTileSize * 0.5f), Quaternion.identity, debugGO.transform);
                    //// Dibujamos líneas en el mundo para crear nuestra cuadrícula.
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y), Color.green, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y), Color.green, 100f);
                }
            }

            Debug.DrawLine(GetWorldPosition(0, iHeight), GetWorldPosition(iWidth, iHeight), Color.green, 100f);
            Debug.DrawLine(GetWorldPosition(iWidth, 0), GetWorldPosition(iWidth, iHeight), Color.green, 100f);
            GridGO = GameObject.Find("GridDebugParent");
            GridGO.transform.position = new Vector3 (7,-1,0);
            GridGO.transform.localScale = new Vector3 (1f, 1, 1f);

        }

    }

    //se inicializa el grit y se guardan los nodos que se utilizaran dentro del array
    public void InitGrid()
    {
        Nodes = new Node[iHeight, iWidth];

        for (int y = 0; y < iHeight; y++)
        {
            for (int x = 0; x < iWidth; x++)
            {
                Nodes[y, x] = new Node(x, y);
            }
        }
    }

    // Se busca un camino entre un nodo inicio y final el cual se lo mandamos anteriormente
    //funciona como cordenadas dentro de un plano
    public List<Node> DepthFirstSearch(int in_startX, int in_startY, int in_endX, int in_endY)
    {
        Node StartNode = GetNode(in_startY, in_startX);
        Node EndNode = GetNode(in_endY, in_endX);

        if (StartNode == null || EndNode == null)
        {
            // Se nos muestra cuando los nodos no se encuentran dentro de la clase
            Debug.LogError("Invalid end or start node in DepthFirstSearch");
            return null;
        }


        Stack<Node> OpenList = new Stack<Node>();
        List<Node> ClosedList = new List<Node>();

        OpenList.Push(StartNode);

        while (OpenList.Count > 0)
        {
            // Mientras haya nodos en la lista abierta, vamos a buscar un camino.
            Node currentNode = OpenList.Pop();
            Debug.Log("Current Node is: " + currentNode.x + ", " + currentNode.y);

            // Checamos si ya llegamos al destino.
            if (currentNode == EndNode)
            {
                // Encontramos un camino.
                Debug.Log("Camino encontrado");
                // Se guarda el camino y se manda al backtrack para imprimirlo en la pantalla 
                List<Node> path = Backtrack(currentNode);
                EnumeratePath(path);
                return path;
            }

            // Otra posible solución
            if (ClosedList.Contains(currentNode))
            {
                continue;
            }

            ClosedList.Add(currentNode);

            // Se visitan los nodos vecinos para encontrar el camino mas corto
            List<Node> currentNeighbors = GetNeighbors(currentNode);
           

            // Metemos los datos a la pila en el orden inverso
            for (int x = currentNeighbors.Count - 1; x >= 0; x--)
            {
                // Solo queremos nodos que no estén en la lista cerrada ya que seran los ya visitados
                if (currentNeighbors[x].bWalkable &&
                    !ClosedList.Contains(currentNeighbors[x]))
                {
                    // Se le asigna el padre al nodo 
                    currentNeighbors[x].Parent = currentNode;  
                    OpenList.Push(currentNeighbors[x]);
                }
            }
           

        }

        //Se muestra un error en caso de no poder encontrar un camino entre el inicio y el final de un nodo
        Debug.LogError("No path found between start and end.");
        return null;
    }

    public List<Node> BreadthFirstSearch(int in_startX, int in_startY, int in_endX, int in_endY)
    {
        Node StartNode = GetNode(in_startY, in_startX);
        Node EndNode = GetNode(in_endY, in_endX);

        if (StartNode == null || EndNode == null)
        {
            // Mensaje de error.
            Debug.LogError("Invalid end or start node in DepthFirstSearch");
            return null;
        }

        Queue<Node> OpenList = new Queue<Node>();
        List<Node> ClosedList = new List<Node>();

        OpenList.Enqueue(StartNode);

        while (OpenList.Count > 0)
        {
            // Mientras haya nodos en la lista abierta, vamos a buscar un camino.
            // Obtenemos el primer nodo de la Lista Abierta
            Node currentNode = OpenList.Dequeue();
            Debug.Log("Current Node is: " + currentNode.x + ", " + currentNode.y);

            // Checamos si ya llegamos al destino.
            if (currentNode == EndNode)
            {
                // Encontramos un camino.
                Debug.Log("Camino encontrado");
                // Necesitamos construir ese camino. Para eso hacemos backtracking.
                List<Node> path = Backtrack(currentNode);
                EnumeratePath(path);
                return path;
            }

            
            if (ClosedList.Contains(currentNode))
            {
                continue;
            }

            ClosedList.Add(currentNode);

            // Se visitan a los vecinos para poder encontrar un camino m
            List<Node> currentNeighbors = GetNeighbors(currentNode);
            foreach (Node neighbor in currentNeighbors)
            {
                if (ClosedList.Contains(neighbor))
                    continue;
                neighbor.Parent = currentNode;
                OpenList.Enqueue(neighbor);
            }

            //nos muesta los nodos dentro de la lista abierta y en donde se ubica
            string RemainingNodes = "Nodes in open list are: ";
            foreach (Node n in OpenList)
                RemainingNodes += "(" + n.x + ", " + n.y + ") - ";
            Debug.Log(RemainingNodes);

        }

        Debug.LogError("No path found between start and end.");
        return null;
    }


    //busca el mejor camino
    public List<Node> BestFirstSearch(int in_startX, int in_startY, int in_endX, int in_endY)
    {
        Node StartNode = GetNode(in_startY, in_startX);
        Node EndNode = GetNode(in_endY, in_endX);

        if (StartNode == null || EndNode == null)
        {
            Debug.LogError("Invalid end or start node in BestFirstSearch");
            return null;
        }

        PriorityQueue OpenList = new PriorityQueue();
        List<Node> ClosedList = new List<Node>();

        OpenList.Add(StartNode);

        while (OpenList.Count > 0)
        {
            //Mientras tengamos nodos en la lista abierta se busca un camino y nos muestra el primer nodo de esta lista
            Node currentNode = OpenList.Dequeue();
            Debug.Log("Current Node is: " + currentNode.x + ", " + currentNode.y);

            
            if (currentNode == EndNode)
            {
                
                Debug.Log("Camino encontrado");
                //Se manda a llamar para que podamos recorrer el camino
                List<Node> path = Backtrack(currentNode);
                EnumeratePath(path);
                return path;
            }

            // Checamos si ya está en la lista cerrada
            if (ClosedList.Contains(currentNode))
            {
                continue;
            }

            ClosedList.Add(currentNode);

            // Vamos a visitar a todos sus vecinos.
            List<Node> currentNeighbors = GetNeighbors(currentNode);
            foreach (Node neighbor in currentNeighbors)
            {
                if (ClosedList.Contains(neighbor))
                    continue;
                // Si no lo contiene, entonces lo agregamos a la lista Abierta
                neighbor.Parent = currentNode;
                int dist = GetDistance(neighbor, EndNode);
                Debug.Log("dist between " + neighbor.x + ", " + neighbor.y + "and goal is: " + dist);
                neighbor.g_Cost = dist;
                OpenList.Insert(dist, neighbor);
            }


        }

        Debug.LogError("No path found between start and end.");
        return null;
    }



    public List<Node> DjikstraSearch(int in_startX, int in_startY, int in_endX, int in_endY)
    {
        Node StartNode = GetNode(in_startY, in_startX);
        Node EndNode = GetNode(in_endY, in_endX);

        if (StartNode == null || EndNode == null)
        {
            // Mensaje de error.
            Debug.LogError("Invalid end or start node in BestFirstSearch");
            return null;
        }

        PriorityQueue OpenList = new PriorityQueue();
        List<Node> ClosedList = new List<Node>();

        StartNode.g_Cost = 0;
        OpenList.Add(StartNode);

        while (OpenList.Count > 0)
        {
            // Mientras haya nodos en la lista abierta, vamos a buscar un camino.
            // Obtenemos el primer nodo de la Lista Abierta
            Node currentNode = OpenList.Dequeue();
            Debug.Log("Current Node is: " + currentNode.x + ", " + currentNode.y);

            // Checamos si ya llegamos al destino.
            // Por motivos didácticos sí lo vamos a terminar al llegar al nodo objetivo.
            if (currentNode == EndNode)
            {
                // Encontramos un camino.
                Debug.Log("Camino encontrado");
                // Necesitamos construir ese camino. Para eso hacemos backtracking.
                List<Node> path = Backtrack(currentNode);
                EnumeratePath(path);
                return path;
            }

            
            if (ClosedList.Contains(currentNode))
            {
                continue;
            }

            ClosedList.Add(currentNode);

            // Vamos a visitar a todos sus vecinos.
            List<Node> currentNeighbors = GetNeighbors(currentNode);
            foreach (Node neighbor in currentNeighbors)
            {
                if (ClosedList.Contains(neighbor))
                    continue;  // podríamos cambiar esto de ser necesario.

                float fCostoTentativo = neighbor.fTerrainCost + currentNode.g_Cost;

                // Si no lo contiene, entonces lo agregamos a la lista Abierta
                // Si ya está en la lista abierta, hay que dejar solo la versión de ese nodo con el 
                // menor costo.
                if (OpenList.Contains(neighbor))
                {
                    // Checamos si este neighbor tiene un costo MENOR que el que ya está en la lista abierta
                    if (fCostoTentativo < neighbor.g_Cost)
                    {
                        // Entonces lo tenemos que remplazar en la lista abierta.
                        OpenList.Remove(neighbor);
                    }
                    else
                    {
                        continue;
                    }
                }

                neighbor.Parent = currentNode;
                neighbor.g_Cost = fCostoTentativo;
                OpenList.Insert((int)fCostoTentativo, neighbor);
            }

            // foreach (Node n in OpenList)
            //    Debug.Log("n Node is: " + n.x + ", " + n.y);

        }

        Debug.LogError("No path found between start and end.");
        return null;
    }


    public List<Node> AStarSearch(int in_startX, int in_startY, int in_endX, int in_endY)
    {
      
        Node StartNode = GetNode(in_startX, in_startY);
        Node EndNode = GetNode(in_endX, in_endY);

        if (StartNode == null || EndNode == null)
        {
            // Mensaje de error.
            Debug.LogError("Invalid end or start node in BestFirstSearch");
            return null;
        }

        PriorityQueue OpenList = new PriorityQueue();
        List<Node> ClosedList = new List<Node>();

        StartNode.g_Cost = 0;
        OpenList.Add(StartNode);

        while (OpenList.Count > 0)
        {
            // Mientras haya nodos en la lista abierta, vamos a buscar un camino.
            // Obtenemos el primer nodo de la Lista Abierta
            Node currentNode = OpenList.Dequeue();
            Debug.Log("Current Node is: " + currentNode.x + ", " + currentNode.y);

            // Checamos si ya llegamos al destino.
            // Por motivos didácticos sí lo vamos a terminar al llegar al nodo objetivo.
            if (currentNode == EndNode)
            {
                // Encontramos un camino.
                Debug.Log("Camino encontrado");
                // Necesitamos construir ese camino. Para eso hacemos backtracking.
                List<Node> path = Backtrack(currentNode);
                
                EnumeratePath(path);
                return path;
            }

            // Checamos si ya está en la lista cerrada
            
            if (ClosedList.Contains(currentNode))
            {
                continue;
            }

            ClosedList.Add(currentNode);

            // Vamos a visitar a todos sus vecinos.
            List<Node> currentNeighbors = GetNeighbors(currentNode);
            foreach (Node neighbor in currentNeighbors)
            {
               
                if (ClosedList.Contains(neighbor))
                {
                    continue;  // podríamos cambiar esto de ser necesario.
                    
                }
                    
                    

                float fCostoTentativo = neighbor.fTerrainCost + currentNode.g_Cost;
                

                // Si no lo contiene, entonces lo agregamos a la lista Abierta
                // Si ya está en la lista abierta, hay que dejar solo la versión de ese nodo con el 
                // menor costo.
                if (OpenList.Contains(neighbor))
                {
                    
                    // Checamos si este neighbor tiene un costo MENOR que el que ya está en la lista abierta
                    if (fCostoTentativo < neighbor.g_Cost)
                    {
                        
                        // Entonces lo tenemos que remplazar en la lista abierta.
                        OpenList.Remove(neighbor);
                        

                    }
                    else
                    {
                        continue;
                    }
                }

                ////se imprime el costo de haber llegado a la casilla
                //Debug.Log("El g_cost es: " + neighbor.g_Cost);
                ////Se imprime el costo del terreno
                //Debug.Log("El costo del terreno es " + neighbor.fTerrainCost);
                ////El costo asociado a la casilla para poder pasar
                //Debug.Log("El costo de la casilla es " + neighbor.h_Cost);
                ////Se imprime la suma de los costos de los terrenos
                //Debug.Log("La suma de los costos es " + neighbor.f_Cost);
                
                neighbor.Parent = currentNode;
                neighbor.g_Cost = fCostoTentativo;
                neighbor.h_Cost = GetDistance(neighbor, EndNode);
                neighbor.f_Cost = neighbor.g_Cost + neighbor.h_Cost;
                OpenList.Insert((int)neighbor.f_Cost, neighbor);
                debugTextArray[neighbor.y, neighbor.x].text = Nodes[neighbor.y, neighbor.x].ToString() + Environment.NewLine + "FCost: " + neighbor.f_Cost.ToString() +
                                                                         Environment.NewLine + "Gcost" + neighbor.g_Cost.ToString() + Environment.NewLine + "Hcost" + neighbor.h_Cost.ToString();
                debugTextArray[neighbor.y, neighbor.x].fontSize = 15;
                debugTextArray[neighbor.y, neighbor.x].color = Color.blue;

            }

            foreach (Node n in OpenList.Nodes)
                Debug.Log("n Node is: " + n.x + ", " + n.y + ", value= " + n.f_Cost);

        }

        Debug.LogError("No path found between start and end.");
        return null;
    }




    public Node GetNode(int x, int y)
    {
        // Checamos si las coordenadas dadas son válidas dentro de nuestra cuadrícula.
        if (x < iWidth && x >= 0 && y < iHeight && y >= 0)
        {
            return Nodes[y, x];
        }

        return null;
    }

    public List<Node> GetNeighbors(Node in_currentNode)
    {
        List<Node> out_Neighbors = new List<Node>();
        // Visitamos al nodo de arriba:
        int x = in_currentNode.x;
        int y = in_currentNode.y;
        if (GetNode(y + 1, x) != null)
        {
            out_Neighbors.Add(Nodes[y + 1, x]);
        }

        // Checamos nodo a la izquierda.
        if (GetNode(y, x - 1) != null)
        {
            out_Neighbors.Add(Nodes[y, x - 1]);
        }

        // Checamos a la derecha
        if (GetNode(y, x + 1) != null)
        {
            out_Neighbors.Add(Nodes[y, x + 1]);
        }

        // Checamos abajo
        if (GetNode(y - 1, x) != null)
        {
            out_Neighbors.Add(Nodes[y - 1, x]);
        }

        return out_Neighbors;
    }

    public List<Node> Backtrack(Node in_node)
    {
        List<Node> out_Path = new List<Node>();
        Node current = in_node;
        while (current.Parent != null)
        {
            out_Path.Add(current);
            current = current.Parent;
        }
        out_Path.Add(current);
        out_Path.Reverse();

        return out_Path;
    }

    // Enumera un camino en el orden que tiene y lo muestra en los debugTextArray.
    public void EnumeratePath(List<Node> in_path)
    {
       
        int iCounter = 0;
        foreach (Node n in in_path)
        {
            iCounter++;
            debugTextArray[n.y, n.x].text = n.ToString() + Environment.NewLine + "FCost: " + n.f_Cost.ToString() + Environment.NewLine + "Hcost" + n.h_Cost + 
                                            Environment.NewLine + "Gcost" + n.g_Cost +
                                            Environment.NewLine + "Step: " + iCounter.ToString();
            debugTextArray[n.y, n.x].color =  Color.red;;
            debugTextArray[n.y, n.x].fontSize = 15;


        }
    }

    public void EnumeratePathCasillas(List<Node> GetNeighborsCost)
    {
        //Cambiamos los colores de las casillas a color azul, esto por medio de los nodos y un recorrido en el for
        foreach (Node n in GetNeighborsCost)
        {
            for (int y = 0; y < iHeight; y++)
            {
                for (int x = 0; x < iWidth; x++)
                {

                    debugTextArray[y, x].text = Nodes[y, x].ToString() + Environment.NewLine + "FCost: " +  n.f_Cost.ToString() + 
                                                                         Environment.NewLine + "Gcost" + n.g_Cost + Environment.NewLine + "Hcost" + n.h_Cost;
                    debugTextArray[y, x].fontSize = 15;
                    debugTextArray[y, x].color = Color.blue;

                }
            }
        }
        //imprime los nodos que seran recorridos a lo largo de la tabla
        //int iCounter = 0;
        //foreach (Node n in in_path)
        //{
        //    iCounter++;
        //    debugTextArray[n.y, n.x].text = n.ToString() + Environment.NewLine + "FCost: " + n.f_Cost.ToString() + "Hcost" + n.g_Cost +
        //        Environment.NewLine + "Step: " + iCounter.ToString();
        //    debugTextArray[n.y, n.x].color = Color.red; ;
        //    debugTextArray[n.y, n.x].fontSize = 15;


        //}



    }
    public int GetDistance(Node in_a, Node in_b)
    {
        // FALTÓ QUE SEAN INT BIEN
        int x_diff = Math.Abs(in_a.x - in_b.x);
        int y_diff = Math.Abs(in_a.y - in_b.y);
        int xy_diff = Math.Abs(x_diff - y_diff);

        return 14 * Math.Min(x_diff, y_diff) + 10 * xy_diff;
    }


    public static TextMesh CreateWorldText2(string in_text, Transform in_parent = null,
        Vector3 in_localPosition = default, int in_iFontSize = 32, Color in_color = default,
        TextAnchor in_textAnchor = TextAnchor.UpperLeft, TextAlignment in_textAlignment = TextAlignment.Left)
    {
        // if (in_color == null) in_color = Color.white;
        if (in_color == null) in_color = Color.gray;

        GameObject MyObject = new GameObject(in_text, typeof(TextMesh));
        MyObject.transform.parent = in_parent;
        MyObject.transform.localPosition = in_localPosition;

        TextMesh myTM = MyObject.GetComponent<TextMesh>();
        myTM.text = in_text;
        myTM.anchor = in_textAnchor;
        myTM.alignment = in_textAlignment;
        myTM.fontSize = in_iFontSize;
        myTM.color = in_color;

        return myTM;
    }


    public Vector3 GetWorldPosition(int x, int y)
    {
        // Nos regresa la posición en mundo del Tile/cuadro especificado por X y Y.
        // Por eso lo multiplicamos por el fTileSize
        // y finalmente sumamos la posición de origen del grid.
        return new Vector3(x, y) * fTileSize + v3OriginPosition;
    }

    public static TextMesh CreateWorldText(string in_text, Transform in_parent = null,
    Vector3 in_localPosition = default, int in_iFontSize = 32,
    Color in_color = default, TextAnchor in_textAnchor = TextAnchor.UpperLeft,
    TextAlignment in_textAlignment = TextAlignment.Left)
    {
        if (in_color == null) in_color = Color.gray;

        // Creamos un GameObject (GO) que tendrá el componente TextMesh donde se mostrará el texto deseado
        GameObject tempGO = new GameObject("World Text", typeof(TextMesh));
        tempGO.transform.SetParent(in_parent);
        tempGO.transform.localPosition = in_localPosition;

        // Inicializamos el componente TextMesh, que es el que realmente se encarga de mostrar en 
        // pantalla el texto que queremos (p.e. el valor del tile, etc.)
        TextMesh textMesh = tempGO.GetComponent<TextMesh>();
        textMesh.anchor = in_textAnchor;
        textMesh.alignment = in_textAlignment;
        textMesh.text = in_text;
        textMesh.fontSize = in_iFontSize;
        textMesh.color = Color.gray;
        return textMesh;
    }

    // Función que convierte una lista de nodos a una lista de puntos en espacio de mundo.
    public List<Vector3> ConvertBacktrackToWorldPos(List<Node> in_path, bool in_shiftToMiddle = true)
    {
        List<Vector3> WorldPositionPoints = new List<Vector3>();

        // Convertimos cada nodo dentro de in_path a una posición en el espacio de mundo.
        foreach (Node n in in_path)
        {
            Vector3 position = GetWorldPosition(n.x, n.y);
            // Si el parámetro in_shiftToMiddle es true, entonces le añadimos para que vaya al centro del nodo.
            if (in_shiftToMiddle)
            {
                position += new Vector3(fTileSize * 0.5f, fTileSize * 0.5f, 0.0f);
            }

            WorldPositionPoints.Add(position);
        }

        return WorldPositionPoints;
    }

}
