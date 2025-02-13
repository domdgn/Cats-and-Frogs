using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public void OnSceneSwitchPress()
    {
        ContainerHandler.ClearAllPositions();
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene != "dragndrop")
        {
            //Debug.LogWarning("DRAG ALLOWED IN SCENE SWITCHER, DONT FORGET TO CHANGE");
            DragManager.isDragAllowed = true;
            SceneManager.LoadScene("dragndrop");
        }
        else
        {
            SceneManager.LoadScene("protoScene");
        }
    }
}
