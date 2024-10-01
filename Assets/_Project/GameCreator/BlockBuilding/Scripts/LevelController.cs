using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class LevelController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] background1Renderer;
    [SerializeField] private SpriteRenderer[] background2Renderer;
    [SerializeField] private SpriteRenderer[] background3Renderer;
    [SerializeField] private SpriteRenderer[] background4Renderer;
    [SerializeField] private SpriteRenderer[] background5Renderer;
    [Space]
    [SerializeField] private Background[] backgrounds;
    [SerializeField] private TMPro.TMP_Dropdown backgroundSelector;
    [Space]
    [SerializeField] private Button btnPlay;
    [SerializeField] private TextMeshProUGUI txtBtnPlay;
    [SerializeField] private GameObject player;
    [Header("Cameras")]
    [SerializeField] private BuildingCamera editorCamera;
    [SerializeField] private CharacterCamera playCamera;

    private GameObject playerInstance;

    public static event EventHandler OnStartLevel;
    public static event EventHandler OnStopLevel;

    public static bool isOnTestPlay = false;
    public static bool hasOpenScreen = false;

    public static LevelController instance;

    private void Awake()
    {
        instance = this;

        backgroundSelector.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 0; i < backgrounds.Length; i++)
        {
            options.Add(backgrounds[i].backgroundName);
        }

        backgroundSelector.AddOptions(options);
        backgroundSelector.onValueChanged.AddListener(OnBackgroundValueChanged);
        btnPlay.onClick.AddListener(BtnPlay_Click);
    }

    private void OnEnable()
    {
        GameOverBlock.OnGameOver += GameOverBlock_OnGameOver;
    }

    private void OnDisable()
    {
        GameOverBlock.OnGameOver -= GameOverBlock_OnGameOver;
    }

    private void GameOverBlock_OnGameOver(object sender, bool e)
    {
        StopScenario();
    }

    private void BtnPlay_Click()
    {
        if(isOnTestPlay)
        {
            StopScenario();
        }
        else
        {
            PlayScenario();
        }
    }

    protected void PlayScenario()
    {
        var spawn = FindObjectOfType<PlayerSpawnBlock>();
        if(spawn == null)
        {
            Debug.LogError("No spawn in this level!");
            return;
        }
        isOnTestPlay = true;
        txtBtnPlay.text = "Stop";
        playerInstance = Instantiate(player, spawn.spawnPoint.position, Quaternion.identity);
        playCamera.target = playerInstance.transform;
        playCamera.gameObject.SetActive(true);
        editorCamera.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);

        OnStartLevel?.Invoke(this, EventArgs.Empty);
    }

    public void StopScenario()
    {
        playCamera.target = null;
        playCamera.gameObject.SetActive(false);
        editorCamera.gameObject.SetActive(true);
        txtBtnPlay.text = "Play";
        isOnTestPlay = false;
        Destroy(playerInstance);
        playerInstance = null;
        OnStopLevel?.Invoke(this, EventArgs.Empty);
    }

    private void OnBackgroundValueChanged(int value)
    {
        UpdateBackgroundSprites(background1Renderer, backgrounds[value].background1);
        UpdateBackgroundSprites(background2Renderer, backgrounds[value].background2);
        UpdateBackgroundSprites(background3Renderer, backgrounds[value].background3);
        UpdateBackgroundSprites(background4Renderer, backgrounds[value].background4);
        UpdateBackgroundSprites(background5Renderer, backgrounds[value].background5);
    }
    private void UpdateBackgroundSprites(SpriteRenderer[] renderers, Sprite newSprite)
    {
        foreach (var renderer in renderers)
        {
            renderer.sprite = newSprite;
        }
    }
}

[System.Serializable]
public class Background
{
    public string backgroundName;
    public Sprite background1;
    public Sprite background2;
    public Sprite background3;
    public Sprite background4;
    public Sprite background5;
}