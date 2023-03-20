using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public GameObject GOStart, GOEnd, GOStarter, GOEnder; //Gameobjects para poder moverlos y deshabilitarlos

    void OnTriggerEnter(Collider collision) //se busca la colision para:
    {
        if (collision.gameObject.CompareTag("Starter")) //mover el nodo inicio al nodo seleccionado dentro de la cuadricula
        {
            GOStart.transform.position = transform.position;
            StartNode.VerifyXStart();//se inicia la verificacion de posicion del nodo inicio
            GOStarter.SetActive(false);//se deshabilita el starter para que no se elimine ni se pongan mas accidentalmente
        }


        if (collision.gameObject.CompareTag("Ender")) //mover el nodo fin al nodo seleccionado dentro de la cuadricula
        {
            GOEnd.transform.position = transform.position;
            EndNode.VerifyXEnd();//se inicia la verificacion de posicion del nodo inicio
            GOEnder.SetActive(false);//se deshabilita el starter para que no se elimine ni se pongan mas accidentalmente
        }
    }
}
