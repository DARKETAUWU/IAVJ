using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEnd : MonoBehaviour
{
    public GameObject GOCamera, GOStarter, GOEnder, GOStarter2, GOEnder2, GODefault;
    public static bool bA1 = false, bA2 = false;

    PathFinding reference;

    private void Start()
    {
        GOCamera.transform.position = new Vector3(44f, 25f, -10f);
    }

    private void Update()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(cursorPos.x, cursorPos.y);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 mouse0Pos = Input.mousePosition;
            {
                GODefault.SetActive(true);
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Vector3 mouse0Pos = Input.mousePosition;
            {
                GODefault.SetActive(false);
            }
        }

        #region Agent1
        if (Input.GetKeyDown(KeyCode.Mouse0) && bA1)
        {
            Vector3 mouse0Pos = Input.mousePosition;
            {
                GOEnder.SetActive(true);
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && bA1)
        {
            Vector3 mouse0Pos = Input.mousePosition;
            {
                GOEnder.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && bA1)
        {
            Vector3 mouse1Pos = Input.mousePosition;
            {
                GOStarter.SetActive(true);
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse1) && bA1)
        {
            Vector3 mouse1Pos = Input.mousePosition;
            {
                GOStarter.SetActive(false);
            }
        }
        #endregion

        #region Agent2
        if (Input.GetKeyDown(KeyCode.Mouse0) && bA2)
        {
            Vector3 mouse0Pos = Input.mousePosition;
            {
                GOEnder2.SetActive(true);
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && bA2)
        {
            Vector3 mouse0Pos = Input.mousePosition;
            {
                GOEnder2.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && bA2)
        {
            Vector3 mouse1Pos = Input.mousePosition;
            {
                GOStarter2.SetActive(true);
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse1) && bA2)
        {
            Vector3 mouse1Pos = Input.mousePosition;
            {
                GOStarter2.SetActive(false);
            }
        }
        #endregion

        if (!bA1)
        {
            GOStarter.SetActive(false);
            GOEnder.SetActive(false);
        }

        if (!bA2)
        {
            GOStarter2.SetActive(false);
            GOEnder2.SetActive(false);
        }
    }

    
}
