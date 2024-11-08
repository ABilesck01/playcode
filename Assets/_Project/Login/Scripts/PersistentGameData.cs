using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PersistentGameData
{
    public static Usuario usuario;
    public static LevelDTO level;

    public static void Logout()
    {
        usuario = null;
        level = null;
    }
}
