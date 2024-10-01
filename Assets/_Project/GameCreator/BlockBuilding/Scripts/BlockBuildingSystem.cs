using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockBuildingSystem : MonoBehaviour
{
    public enum BrushType
    {
        brush,
        eraser
    }

    [SerializeField] private BlockAsset currentBlock;
    
    private BrushType brushType;
    private Grid<GridObject> grid;

    public static BlockBuildingSystem instance;
    private bool use = true;

    public class BlockEventArgs : EventArgs
    {
        public BaseBlock block;
    }

    public static event EventHandler<BlockEventArgs> OnPlaceBlock;
    public static event EventHandler<BlockEventArgs> OnEraseBlock;

    public Grid<GridObject> Grid() => grid;

    private void OnEnable()
    {
        LevelController.OnStartLevel += LevelController_OnStartLevel;
        LevelController.OnStopLevel += LevelController_OnStopLevel;
    }

    private void OnDisable()
    {
        LevelController.OnStartLevel -= LevelController_OnStartLevel;
        LevelController.OnStopLevel -= LevelController_OnStopLevel;
    }

    private void LevelController_OnStartLevel(object sender, System.EventArgs e)
    {
        use = false;
    }

    private void LevelController_OnStopLevel(object sender, System.EventArgs e)
    {
        use = true;
    }

    void Awake()
    {
        instance = this;    

        grid = new Grid<GridObject>(40, 20, 1, transform.position, 
            (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y), true);
    }

    // Update is called once per frame
    void Update()
    {
        if(!use) return;

        if (Input.GetMouseButton(0))
        { 
            if(brushType == BrushType.brush)
            {
                Brush();
            }
            else if(brushType == BrushType.eraser)
            {
                Erase();
            }
        }
    }

    private void Brush()
    {
        if (MouseOverUI()) return;

        grid.GetXY(GetMouseWorldPosition(), out int x, out int y);

        GridObject gridObject = grid.GetValue(x, y);

        if (!gridObject.CanBuild())
        {
            return;
        }
        var block = BaseBlock.Place(currentBlock, grid.GetWorldPosition(x, y), new Vector2Int(x, y));
        OnPlaceBlock?.Invoke(this, new BlockEventArgs
        {
            block = block
        });
        gridObject.SetBlockObject(block);
    }

    private void Erase()
    {
        if (MouseOverUI()) return;

        grid.GetXY(GetMouseWorldPosition(), out int x, out int y);

        GridObject gridObject = grid.GetValue(x, y);

        if (gridObject.CanBuild())
        {
            return;
        }

        OnEraseBlock?.Invoke(this, new BlockEventArgs
        {
            block = gridObject.GetPlacedObject()
        });
        Destroy(gridObject.GetPlacedObject().gameObject);
        gridObject.ClearPlacedObject();
        
    }

    public BaseBlock SystemPlaceBlock(BlockAsset asset, int x, int y)
    {

        GridObject gridObject = grid.GetValue(x, y);
        var block = BaseBlock.Place(asset, grid.GetWorldPosition(x, y), new Vector2Int(x, y));
        gridObject.SetBlockObject(block);
        return block;
    }

    public void SetBlockAsset(BlockAsset block)
    {
        this.currentBlock = block;
    }

    public void SetBrush(BrushType brushType)
    {
        this.brushType = brushType;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        return mousePosition;
    }

    private bool MouseOverUI()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);



        return raycastResults.Count > 0;
    }
}
