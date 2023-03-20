using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartNode : MonoBehaviour
{
   public static int _Xstart = 1, _Ystart = 4; //Se inicializan los ints para asignarles las coordenadas

    public GameObject GOStarter, GOEnder; //Se asignan variables a los game objects para poder desabilitarlos

    public static Transform TStart; //se asigna el transform para poder usarlo en static

    void Start()
    {
        TStart = GetComponent < Transform >(); //Se toma el componente transform del game object para poder utilizarlo
    }

    public static void VerifyXStart() //se verifica la posicion del nodo inicio para darle valor a las coordenadas y mandarlas a "ASunObject"
    {
        Debug.Log(TStart.position); //debug para saber las coordenadas del nodo inicio

        if (TStart.position.x >= 11f && TStart.position.x  <= 12f)
        {
           _Xstart = 0;
        }

        if (TStart.position.x >= 22f && TStart.position.x <= 23f)
        {
            _Xstart = 1;
        }

        if (TStart.position.x >= 32f && TStart.position.x <= 33f)
        {
            _Xstart = 2;
        }

        if (TStart.position.x >= 42f && TStart.position.x <= 43f)
        {
            _Xstart = 3;
        }

        if (TStart.position.x >= 52f && TStart.position.x <= 53f)
        {
            _Xstart = 4;
        }

        if (TStart.position.y >= 3f && TStart.position.y <= 4)
        {
            _Ystart = 0;
        }

        if (TStart.position.y >= 14f && TStart.position.y <= 15f)
        {
            _Ystart = 1;
        }

        if (TStart.position.y >= 23f && TStart.position.y <= 24f)
        {
            _Ystart = 2;
        }

        if (TStart.position.y >= 33f && TStart.position.y <= 34f)
        {
            _Ystart = 3;
        }

        if (TStart.position.y >= 43f && TStart.position.y <= 44f)
        {
            _Ystart = 4;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Starter") || collision.gameObject.CompareTag("Ender")) //Se busca a colision de trigger para desabilitar el nodo y los gameobjects Ender y Starter
        {
            TStart.position = new Vector3(145.441772f, 3.31230211f, 0f);
            GOEnder.SetActive(false);
            GOStarter.SetActive(false);
        }
    }
}
