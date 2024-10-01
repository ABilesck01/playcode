using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanvas : MonoBehaviour
{
    [Serializable]
    public class GameOverView : BaseView
    {
        
    }

    [Serializable]
    public class WinView : BaseView
    {
    }

    public GameOverView gameOverView;
    public WinView winView;

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

    public virtual void ShowView()
    {
        view.SetActive(true);
    }

    public virtual void HideView()
    {
        view.SetActive(false);
    }
}
