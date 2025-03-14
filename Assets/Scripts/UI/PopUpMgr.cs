using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpMgr : MonoBehaviour
{
    private int slideIndex = 0;
    private UIManager uiMgr;
    [SerializeField] private GameObject slideImage;
    [SerializeField] private List<Sprite> images;
    private Image UIImage;
    [SerializeField] private CanvasGroup gameUI;
    private WaveManager waveManager;

    private void Awake()
    {
        transform.parent.gameObject.SetActive(true);
        UIImage = slideImage.GetComponent<Image>();
        UIImage.sprite = images[slideIndex];
        uiMgr = UIManager.Instance;
        waveManager = FindObjectOfType<WaveManager>();
    }

    public void NextSlide()
    {
        slideIndex++;

        if (slideIndex >= images.Count)
        {
            ClosePopUp();
        }
        else
        {
            UIImage.sprite = images[slideIndex];
        }
    }

    private void ClosePopUp()
    {
        gameUI.interactable = true;
        transform.parent.gameObject.SetActive(false);
        waveManager.BeginGame();
        Debug.Log("game begun");
    }
}
