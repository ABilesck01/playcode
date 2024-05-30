using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public List<BaseBlockData> blocks;

    public LevelData() 
    {
        blocks = new List<BaseBlockData>();
    }

    public void AddBlock(BaseBlock block)
    {
        BaseBlockData data = new BaseBlockData();
        data.assetId = block.blockSO.Id;
        data.x = block.x;
        data.y = block.y;

        if (block is EventBlock)
        {
            data.customData = ((EventBlock)block).GetData();
        }

        blocks.Add(data);
    }

    public void RemoveBlock(BaseBlock block)
    {
        BaseBlockData data = new BaseBlockData();
        data.assetId = block.blockSO.Id;
        data.x = block.x;
        data.y = block.y;

        blocks.Remove(data);
    }
}

[System.Serializable]
public class BaseBlockData
{
    public string assetId;
    public int x;
    public int y;
    public string customData;
}
[System.Serializable]
public class EventBlockData
{
    public int spriteIndex;
    public int eventTrigger;
    public List<ActionData> actions;

    public EventBlockData() : base()
    {
        actions = new List<ActionData>();
    }

    public EventBlockData(EventBlock eventBlock)
    {
        this.spriteIndex = eventBlock.CurrentSprite;
        this.eventTrigger = eventBlock.CurrentTrigger;
        this.actions = new List<ActionData>();

        foreach (var action in eventBlock.GetActions)
        {
            this.actions.Add(new ActionData(action));
        }
    }
}

[System.Serializable]
public class ActionData
{
    public string type;
    public Dictionary<string, object> parameters;

    public ActionData(BaseAction action)
    {
        this.type = action.GetType().Name;
        this.parameters = new Dictionary<string, object>();

        if (action is MoveAction moveAction)
        {
            parameters.Add("Direction", moveAction.Direction.ToString());
        }
        else if (action is SetVariableAction setVariableAction)
        {
            parameters.Add("VariableName", setVariableAction.VariableName);
            parameters.Add("VariableValue", setVariableAction.VariableValue);
            parameters.Add("Operation", setVariableAction.Operation);
        }
        else if (action is IfElseAction ifElseAction)
        {
            parameters.Add("LeftVariable", ifElseAction.LeftVariable);
            parameters.Add("RightVariable", ifElseAction.RightVariable);
            parameters.Add("ComparisonOperator", ifElseAction.Comparison);
            parameters.Add("RightVariableValue", ifElseAction.RightVariableValue);
            parameters.Add("RightVariableExplicit", ifElseAction.ExplicitRightVariable);

            if(ifElseAction.IfAction != null)
            {
                parameters.Add("ifAction", new ActionData(ifElseAction.IfAction));
            }
        }
    }
}