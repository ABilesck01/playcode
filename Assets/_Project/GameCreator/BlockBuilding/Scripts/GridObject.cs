using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private Grid<GridObject> grid;
    private int x;
    private int y;
    private BaseBlock block;

    public GridObject(Grid<GridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void SetBlockObject(BaseBlock b)
    {
        Debug.Log(b.gameObject.name);
        block = b;
    }

    public void ClearPlacedObject()
    {
        block = null;
    }

    public BaseBlock GetPlacedObject()
    {
        return this.block;
    }

    public bool CanBuild()
    {
        return block == null;
    }
}
