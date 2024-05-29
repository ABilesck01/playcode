using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBlock : MonoBehaviour
{
    public static BaseBlock Place(BlockAsset blockSO, Vector3 position, Vector2Int origin)
    {
        BaseBlock block = Instantiate(blockSO.blockPrefab, position, Quaternion.identity);
        block.blockSO = blockSO;
        block.x = origin.x;
        block.y = origin.y;

        return block;
    }

    public BlockAsset blockSO;
    public int x;
    public int y;
}
