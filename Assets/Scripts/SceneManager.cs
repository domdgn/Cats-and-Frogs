using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public void OnSceneSwitchPress()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene != "dragndrop")
        {
            SceneManager.LoadScene("dragndrop");
        }
        else
        {
            SceneManager.LoadScene("protoScene");
        }
    }
}
