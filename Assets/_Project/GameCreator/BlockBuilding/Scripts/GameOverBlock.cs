using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GameOverBlock : BaseBlock
{
    [SerializeField] private bool hasWon;

    public static event EventHandler<bool> OnGameOver;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OnGameOver?.Invoke(this, hasWon);
        }
    }

    public static void CallGameOver(bool hasWon)
    {
        OnGameOver?.Invoke(null, hasWon);
    }
}
