using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndNode : MonoBehaviour
{
    public static int _Xend = 4, _Yend = 1; //Se inicializan los ints para asignarles las coordenadas

    public GameObject GOStarter, GOEnder; //Se asignan variables a los game objects para poder desabilitarlos

    public static Transform TEnd; //se asigna el transform para poder usarlo en static

    void Start()
    {
        TEnd = GetComponent<Transform>();  //Se toma el componente transform del game object para poder utilizarlo
    }

    public static void VerifyXEnd()  //se verifica la posicion del nodo inicio para darle valor a las coordenadas y mandarlas a "ASunObject"
    {
        Debug.Log(TEnd.position); //debug para saber las coordenadas del nodo fin

        if (TEnd.position.x >= 11f && TEnd.position.x <= 12f)
        {
            _Xend = 0;
        }

        if (TEnd.position.x >= 22f && TEnd.position.x <= 23f)
        {
            _Xend = 1;
        }

        if (TEnd.position.x >= 32f && TEnd.position.x <= 33f)
        {
            _Xend = 2;
        }

        if (TEnd.position.x >= 42f && TEnd.position.x <= 43f)
        {
            _Xend = 3;
        }

        if (TEnd.position.x >= 52f && TEnd.position.x <= 53f)
        {
            _Xend = 4;
        }

        if (TEnd.position.y >= 3f && TEnd.position.y <= 4)
        {
            _Yend = 0;
        }

        if (TEnd.position.y >= 14f && TEnd.position.y <= 15f)
        {
            _Yend = 1;
        }

        if (TEnd.position.y >= 23f && TEnd.position.y <= 24f)
        {
            _Yend = 2;
        }

        if (TEnd.position.y >= 33f && TEnd.position.y <= 34f)
        {
            _Yend = 3;
        }

        if (TEnd.position.y >= 43f && TEnd.position.y <= 44f)
        {
            _Yend = 4;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Starter") || collision.gameObject.CompareTag("Ender"))  //Se busca a colision de trigger para desabilitar el nodo y los gameobjects Ender y Starter
        {
            TEnd.position = new Vector3(138.100006f, 3.5f, 0f);
            GOEnder.SetActive(false);
            GOStarter.SetActive(false);
        }
    }
}
