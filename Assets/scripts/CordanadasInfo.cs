using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CordanadasInfo : MonoBehaviour
{
    //tomamos la cordenada en donde se encuentra
    public int Xnode, Ynode;

    //Manda la informacion al agente
    ASunAgent _ASunAgentReference;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Start"))
        {
           
            _ASunAgentReference._Xstart = Xnode;
            _ASunAgentReference._Ystart = Ynode;

        }

        if (other.gameObject.CompareTag("End"))
        {
            
            _ASunAgentReference._Yend = Xnode;
            _ASunAgentReference._Xend = Ynode;
        }
    }
   
}
