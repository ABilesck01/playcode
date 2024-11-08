using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour
{
    [Serializable]
    public class GameOverView : BaseView
    {
        public Button btnReplay;
        public Button btnQuit;

        private void BtnReplayClick()
        {
            SceneManager.LoadScene("PlayGame");
        }

        private void BtnQuitClick()
        {
            SceneManager.LoadScene("Main");
        }

        public override void InitView()
        {
            base.InitView();
            btnReplay.onClick.AddListener(BtnReplayClick);
            btnQuit.onClick.AddListener(BtnQuitClick);
        }
    }

    [Serializable]
    public class WinView : GameOverView
    {
        public Button btnLike;
        public Button btnDislike;

        public override void InitView()
        {
            base.InitView();
            btnLike.onClick.AddListener(BtnLikeClick);
            btnDislike.onClick.AddListener(BtnDeslikeClick);
        }

        private void BtnDeslikeClick()
        {
            AvaliacaoDto avaliacaoDto = new AvaliacaoDto
            {
                UsuarioId = PersistentGameData.usuario.ID,
                gostei = false
            };

            btnLike.interactable = false;
            btnDislike.interactable = false;

            ApiController.instance.SendRequest<Message>(RequestType.POST, $"UserLevel/{PersistentGameData.level.id}/avaliar", OnDeslikeSuccess, OnError, avaliacaoDto);
        }

        private void OnError(string obj)
        {
            Debug.LogError(obj);
        }

        private void OnDeslikeSuccess(Message message)
        {
            Debug.Log(message.message);
        }

        private void BtnLikeClick()
        {
            AvaliacaoDto avaliacaoDto = new AvaliacaoDto
            {
                UsuarioId = PersistentGameData.usuario.ID,
                gostei = true
            };

            btnLike.interactable = false;
            btnDislike.interactable = false;

            ApiController.instance.SendRequest<Message>(RequestType.POST, $"UserLevel/{PersistentGameData.level.id}/avaliar", OnDeslikeSuccess, OnError, avaliacaoDto);
        }
    }

    public GameOverView gameOverView;
    public WinView winView;

    private void Start()
    {
        gameOverView.InitView();
        winView.InitView();
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
        if(e)
        {
            winView.ShowView();
        }
        else
        {
            gameOverView.ShowView();
        }
    }
}

public class BaseView
{
    public GameObject view;

    public virtual void InitView()
    {

    }

    public virtual void ShowView()
    {
        view.SetActive(true);
    }

    public virtual void HideView()
    {
        view.SetActive(false);
    }
}
public class AvaliacaoDto
{
    public int UsuarioId;
    public bool gostei;
}