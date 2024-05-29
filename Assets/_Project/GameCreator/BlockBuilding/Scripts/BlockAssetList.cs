using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Blocks/New Block List")]
public class BlockAssetList : ScriptableObject
{
    public List<BlockAsset> blockAssets;

    public BlockAsset GetBlock(string id)
    {
        var asset = blockAssets.Find(b => b.Id.Equals(id));
        return asset;
    }
}
