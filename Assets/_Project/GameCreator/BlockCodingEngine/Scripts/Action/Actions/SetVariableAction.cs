using System;
using UnityEngine;

[Serializable]
public class SetVariableAction : BaseAction
{
    private string variableName;
    private int variableValue;
    private VariableOperation operation; // Adiciona um campo para a operação

    public string VariableName
    {
        get { return variableName; }
        set { variableName = value; }
    }

    public int VariableValue
    {
        get { return variableValue; }
        set { variableValue = value; }
    }

    public VariableOperation Operation
    {
        get { return operation; }
        set { operation = value; }
    }

    public SetVariableAction()
    {
        this.Name = "Variavel";
        this.TutorialText = "Use this command to change a value in memory.";
    }

    public override void Execute(EventBlock block)
    {
        int currentValue = VariableController.instance.GetVariable(variableName);

        switch (operation)
        {
            case VariableOperation.Set:
                VariableController.instance.SetVariable(variableName, variableValue);
                break;
            case VariableOperation.Add:
                VariableController.instance.SetVariable(variableName, currentValue + variableValue);
                break;
            case VariableOperation.Subtract:
                VariableController.instance.SetVariable(variableName, currentValue - variableValue);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(operation), "Unsupported variable operation.");
        }

        Debug.Log($"Operation {operation} applied on variable {variableName} with result {VariableController.instance.GetVariable(variableName)}");
    }
}

public enum VariableOperation
{
    Set, // Define o valor diretamente
    Add, // Soma um valor ao valor atual da variável
    Subtract // Subtrai um valor do valor atual da variável
}
