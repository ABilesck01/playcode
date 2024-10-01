using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int id;
    public string nome;
    public string usuarioNome;
    public List<Bloco> blocos;
    //public List<string> variables;

    public LevelData() 
    {
        blocos = new List<Bloco>();
        //variables = new List<string>();
    }

    public void AddBlock(BaseBlock block)
    {
        Bloco data = new Bloco();
        data.id_Interno = block.blockSO.ID;
        data.x = block.x;
        data.y = block.y;

        //if (block is EventBlock)
        //{
        //    data.customData = ((EventBlock)block).GetData();
        //}

        blocos.Add(data);
    }

    public void RemoveBlock(BaseBlock block)
    {
        Bloco data = new Bloco();
        data.id_Interno = block.blockSO.ID;
        data.x = block.x;
        data.y = block.y;

        blocos.Remove(data);
    }
}

[System.Serializable]
public class Bloco
{
    public int id;
    public string id_Interno;
    public int sprite_Index;
    public int x;
    public int y;
    public string customData;

}
[System.Serializable]
public class EventBlockData
{
    public int spriteIndex;
    public int eventTrigger;
    public bool isLoop;
    public bool isSolid;
    public List<ActionData> actions;

    public EventBlockData() : base()
    {
        actions = new List<ActionData>();
    }

    public EventBlockData(EventBlock eventBlock)
    {
        this.spriteIndex = eventBlock.CurrentSprite;
        this.eventTrigger = eventBlock.CurrentTrigger;
        this.isLoop = eventBlock.isLooped;
        this.isSolid = eventBlock.isSolid;
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

    public ActionData()
    {
        type = "";
        parameters = new Dictionary<string, object>();
    }

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
        else if(action is ShowTextAction showTextAction)
        {
            parameters.Add("display", showTextAction.Display);
        }
    }
}

public class SaveLevelDto
{
    public int userLevelId;
    public List<Bloco> blocos;
    public Variavel[] variaveis;
}

[System.Serializable]
public class Variavel
{
    public string nome;
}