using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpMgr : MonoBehaviour
{
    private int slideIndex = 0;
    private UIManager uiMgr;
    [SerializeField] private GameObject slideImage;
    [SerializeField] private GameObject introBtn;
    [SerializeField] private GameObject failText;
    [SerializeField] private GameObject failBtn;
    [SerializeField] private List<Sprite> images;
    private Image UIImage;
    [SerializeField] private CanvasGroup gameUI;
    private WaveManager waveManager;
    private Canvas canvas;

    private void Awake()
    {
        canvas = transform.parent.GetComponent<Canvas>();
        UIImage = slideImage.GetComponent<Image>();
        UIImage.sprite = images[slideIndex];
        uiMgr = UIManager.Instance;
        waveManager = FindObjectOfType<WaveManager>();

        slideImage.SetActive(true);
        introBtn.SetActive(true);
        failBtn.SetActive(false);
        failText.SetActive(false);

        PondScript pondScript = FindObjectOfType<PondScript>();
        if (pondScript != null)
        {
            pondScript.OnGameOver += ShowFailPopUp;
        }
    }

    public void NextSlide()
    {
        AudioPlayer.Instance.PlaySFX(AudioPlayer.Instance.select);
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
        canvas.gameObject.SetActive(false);
        waveManager.BeginGame();
    }

    public void ShowFailPopUp()
    {
        canvas.gameObject.SetActive(true);
        slideImage.SetActive(false);
        introBtn.SetActive(false);
        failBtn.SetActive(true);
        failText.SetActive(true);
    }
}
