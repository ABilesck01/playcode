using System;
using UnityEngine;

[System.Serializable]
public class IfElseAction : BaseAction
{
    [SerializeField] private string leftVariable;
    [SerializeField] private string rightVariable;
    [SerializeField] private ComparisonOperator comparisonOperator;
    [SerializeField] private BaseAction ifAction;
    [SerializeField] private BaseAction elseAction;

    private bool explicitRightVariable;

    public bool ExplicitRightVariable
    {
        get { return explicitRightVariable; }
        set { explicitRightVariable = value; }
    }


    private int rightVariableValue;

    public int RightVariableValue
    {
        get { return rightVariableValue; }
        set { rightVariableValue = value; }
    }


    public string LeftVariable { get => leftVariable; set => leftVariable = value; }
    public string RightVariable { get => rightVariable; set => rightVariable = value; }
    public ComparisonOperator Comparison { get => comparisonOperator; set => comparisonOperator = value; }
    public BaseAction IfAction { get => ifAction; set => ifAction = value; }
    public BaseAction ElseAction { get => elseAction; set => elseAction = value; }

    public enum ComparisonOperator
    {
        LessThan,
        GreaterThan,
        LessThanOrEqual,
        GreaterThanOrEqual,
        Equal
    }

    public IfElseAction()
    {
        id = Guid.NewGuid();
        this.Name = "Condi��o";
    }

    public override void Execute(EventBlock block)
    {
        if (EvaluateCondition())
        {
            ifAction?.Execute(block);
        }
    }

    private bool EvaluateCondition()
    {
        int leftValue = VariableController.instance.GetVariable(leftVariable);
        int rightValue = explicitRightVariable ? rightVariableValue : VariableController.instance.GetVariable(rightVariable);

        switch (comparisonOperator)
        {
            case ComparisonOperator.LessThan:
                return leftValue < rightValue;
            case ComparisonOperator.GreaterThan:
                return leftValue > rightValue;
            case ComparisonOperator.LessThanOrEqual:
                return leftValue <= rightValue;
            case ComparisonOperator.GreaterThanOrEqual:
                return leftValue >= rightValue;
            case ComparisonOperator.Equal:
                return leftValue == rightValue;
            default:
                throw new InvalidOperationException("Unsupported comparison operator.");
        }
    }
}
