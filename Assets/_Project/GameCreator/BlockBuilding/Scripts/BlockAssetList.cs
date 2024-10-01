using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Blocks/New Block List")]
public class BlockAssetList : ScriptableObject
{
    public string idPrefix = "item";
    public List<BlockAsset> blockAssets;

    public BlockAsset GetBlock(string id)
    {
        var asset = blockAssets.Find(b => b.ID.Equals(id));
        return asset;
    }

    private void OnValidate()
    {
        for (int i = 0; i < blockAssets.Count; i++)
        {
            if (string.IsNullOrEmpty(blockAssets[i].ID))
            {
                blockAssets[i].GenerateID(idPrefix, i + 1);
                Debug.Log($"ID gerado: {blockAssets[i].ID} para o objeto {blockAssets[i].name}");
            }
        }
    }
}
