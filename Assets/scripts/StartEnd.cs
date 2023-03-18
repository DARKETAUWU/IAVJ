using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEnd : MonoBehaviour
{
    public GameObject GOCamera;

    public string sTagString;

    private void Start()
    {
        GOCamera.transform.position = new Vector3(44f, 25f, -10f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == sTagString)
        {
            
        }
    }

    //private void Update()
    //{
    //    Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    transform.position = new Vector2(cursorPos.x, cursorPos.y);
    //}

    //public void InitPos()
    //{
    //    //Si esta marcado y se usa click izquierdo:
    //    if (Input.GetKey(KeyCode.Mouse0))
    //    {
    //        Vector3 mouse0Pos = Input.mousePosition;
    //        {
    //        }
    //    }
    //}

    //public void EndPos()
    //{
    //    //Si esta marcado y se usa click derecho:
    //    if (Input.GetKey(KeyCode.Mouse1))
    //    {
    //        Vector3 mouse1Pos = Input.mousePosition;
    //        {
    //        }
    //    }
    //}
}
