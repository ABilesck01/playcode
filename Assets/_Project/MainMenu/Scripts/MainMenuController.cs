using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Buttons")]
    public Button btnCreate;
    public Button btnPlay;
    public Button btnProfile;
    public Button btnTutorial;
    public Button[] btnBackToMain;

    [Header("Screens")]
    public GameObject mainScreen;
    public GameObject createScreen;
    public GameObject playScreen;
    public GameObject profileScreen;
    public GameObject tutorialScreen;

    private void Awake()
    {
        btnCreate.onClick.AddListener(BtnCreateClick);
        btnPlay.onClick.AddListener(BtnPlayClick);
        btnProfile.onClick.AddListener(BtnProfileClick);
        btnTutorial.onClick.AddListener(BtnTutorialClick);
        for (int i = 0; i < btnBackToMain.Length; i++)
        {
            btnBackToMain[i].onClick.AddListener(BackToMainClick);
        }

    }

    private void BackToMainClick()
    {
        mainScreen.SetActive(true);
        createScreen.SetActive(false);
        playScreen.SetActive(false);
        profileScreen.SetActive(false);
    }

    private void BtnCreateClick()
    {
        mainScreen.SetActive(false);
        createScreen.SetActive(true);
    }

    private void BtnPlayClick()
    {
        mainScreen.SetActive(false);
        playScreen.SetActive(true);
    }

    private void BtnProfileClick()
    {
        mainScreen.SetActive(false);
        profileScreen.SetActive(true);
    }

    private void BtnTutorialClick()
    {
        mainScreen.SetActive(false);
        tutorialScreen.SetActive(true);
    }
}
