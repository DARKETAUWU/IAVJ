using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Default : MonoBehaviour
{
    public void loadScene()
    {
        SceneManager.LoadScene(1);
    }
}
