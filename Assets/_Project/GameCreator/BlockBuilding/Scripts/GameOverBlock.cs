using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GameOverBlock : BaseBlock
{
    public static event EventHandler<bool> OnGameOver;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OnGameOver?.Invoke(this, true);
        }
    }

    public static void CallGameOver(bool hasWon)
    {
        OnGameOver?.Invoke(null, hasWon);
    }
}
