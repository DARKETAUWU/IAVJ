using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public GameObject GOStart, GOEnd;
    public bool bStart = false, bEnd = false;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Starter"))
        {
            if (bEnd == true)
            {
                GOEnd.transform.position = new Vector3(95.9400024f, 3.80999994f, 0f);
                bEnd = false;
            }
            GOStart.transform.position = transform.position;
            bStart = true;
        }

        if (collision.gameObject.CompareTag("Ender"))
        {
            if (bStart == true)
            {
                GOStart.transform.position = new Vector3(103.281769f, 3.31230211f, 0f);
                bStart = false;
            }
            GOEnd.transform.position = transform.position;
            bEnd = true;
        }
    }
}
