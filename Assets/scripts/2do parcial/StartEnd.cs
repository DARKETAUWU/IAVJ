using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEnd : MonoBehaviour
{
    public GameObject GOCamera, GOStarter, GOEnder, GOStarter2, GOEnder2, GODefault; 

    public static bool bA1 = false, bA2 = false; //Bools para saber de que agente se trata

    PathFinding reference; //por si explota lo dejamos

    private void Start()
    {
        GOCamera.transform.position = new Vector3(44f, 25f, -10f);//se acomoda la camara para que se vea toda la cuadricula
    }

    private void Update()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//esto es para que los nodos iniciador y finalizador sigan el mouse
        transform.position = new Vector2(cursorPos.x, cursorPos.y);//esto hace que se muevan los nodos

        if (Input.GetKeyDown(KeyCode.Mouse0)) //cuando se presiona click izquierdo:
        {
            Vector3 mouse0Pos = Input.mousePosition;
            {
                GODefault.SetActive(true); //se activa el default para seleccionar el agente
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0)) //cuando se deja de presionar click izquierdo:
        {
            Vector3 mouse0Pos = Input.mousePosition;
            {
                GODefault.SetActive(false);  //se desactiva el default para seleccionar el agente
            }
        }

        #region Agent1
        if (Input.GetKeyDown(KeyCode.Mouse0) && bA1) //cuando se presiona click izquierdo y esta seleccionado el agente 1:
        {
            Vector3 mouse0Pos = Input.mousePosition;
            {
                GOEnder.SetActive(true); //se activa el nodo finalizador
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && bA1) //cuando se deja de presionar click izquierdo y esta seleccionado el agente 1:
        {
            Vector3 mouse0Pos = Input.mousePosition;
            {
                GOEnder.SetActive(false);// se desactiva el nodo finalizador
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && bA1) //cuando se presiona click derecho y esta seleccionado el agente 1:
        {
            Vector3 mouse1Pos = Input.mousePosition; 
            {
                GOStarter.SetActive(true); //se activa el nodo iniciador
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse1) && bA1) //cuando se deja de presionar click derecho y esta seleccionado el agente 1:
        {
            Vector3 mouse1Pos = Input.mousePosition;
            {
                GOStarter.SetActive(false); //se desactiva el nodo iniciador
            }
        }
        #endregion

        #region Agent2
        if (Input.GetKeyDown(KeyCode.Mouse0) && bA2) //cuando se presiona click izquierdo y esta seleccionado el agente 2:
        {
            Vector3 mouse0Pos = Input.mousePosition;
            {
                GOEnder2.SetActive(true); //se activa el nodo finalizador 2
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && bA2) //cuando se deja de presionar click izquierdo y esta seleccionado el agente 2:
        {
            Vector3 mouse0Pos = Input.mousePosition;
            {
                GOEnder2.SetActive(false); // se desactiva el nodo finalizador 2
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && bA2) //cuando se presiona click derecho y esta seleccionado el agente 2:
        {
            Vector3 mouse1Pos = Input.mousePosition;
            {
                GOStarter2.SetActive(true); //se activa el nodo iniciador 2
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse1) && bA2) //cuando se deja de presionar click derecho y esta seleccionado el agente 2:
        {
            Vector3 mouse1Pos = Input.mousePosition;
            {
                GOStarter2.SetActive(false); //se desactiva el nodo iniciador 2
            }
        }
        #endregion

        if (!bA1)
        {
            GOStarter.SetActive(false);//nodo iniciador se desactiva
            GOEnder.SetActive(false); //nodo finalizador se desactiva
        }

        if (!bA2)
        {
            GOStarter2.SetActive(false);//nodo iniciador 2 se desactiva
            GOEnder2.SetActive(false); //nodo finalizador 2 se desactiva
        }
    }

    
}
