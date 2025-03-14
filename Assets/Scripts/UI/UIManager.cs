using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    private PopUpMgr popUpMgr;
    private CanvasGroup gameUI;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void BeginGame()
    {
        //add transition
        SceneManager.LoadScene("Game");
        AudioPlayer.Instance.PlaySFX(AudioPlayer.Instance.coinCollect);
    }

    //public void ChangeInteractability()
    //{
    //    gameUI = FindObjectOfType<CanvasGroup>();
    //    if (gameUI == null)
    //    {
    //        Debug.LogError("CanvasGroup not found in the scene!");
    //    }
    //    gameUI.interactable = !gameUI.interactable;
    //}
}
