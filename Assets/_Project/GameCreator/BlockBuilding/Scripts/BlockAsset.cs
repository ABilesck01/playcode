using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Blocks/New Block")]
public class BlockAsset : ScriptableObject
{
    public string Id;
    public Sprite preview;
    public BaseBlock blockPrefab;

    void OnEnable()
    {
        if (string.IsNullOrEmpty(Id))
            Id = Guid.NewGuid().ToString();
    }
}
