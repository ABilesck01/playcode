using UnityEngine;
using System;
using UnityEditor;
using Unity.Collections;

[CreateAssetMenu(menuName = "Blocks/New Block")]
public class BlockAsset : ScriptableObject
{
    [ReadOnly] public string ID;
    public Sprite preview;
    public BaseBlock blockPrefab;

    public void GenerateID(string prefix, int index)
    {
        ID = $"{prefix}_{index}";
    }
}
