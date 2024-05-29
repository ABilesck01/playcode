using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetVariableController : BaseController
{
    public TMP_Dropdown variableDropdown;
    public TMP_Dropdown operationDropdown;
    public TMP_InputField valueInputField;

    private void Awake()
    {
        PopulateVariableDropdown();
        PopulateOperationDropdown();

        variableDropdown.onValueChanged.AddListener(OnVariableChange);
        operationDropdown.onValueChanged.AddListener(OnOperationChange);
        valueInputField.onValueChanged.AddListener(OnValueChange);
    }

    private void Start()
    {
        if (variableDropdown.options.Count > 0)
        {
            string selectedVariable = variableDropdown.options[0].text;
            ((SetVariableAction)action).VariableName = selectedVariable;
        }
    }

    private void OnEnable()
    {
        VariableController.onVariablesChanged += VariableController_onVariablesChanged;
        PopulateVariableDropdown();
        PopulateOperationDropdown();
    }

    private void OnDisable()
    {
        VariableController.onVariablesChanged -= VariableController_onVariablesChanged;
    }

    private void VariableController_onVariablesChanged()
    {
        PopulateVariableDropdown();
    }

    private void PopulateVariableDropdown()
    {
        variableDropdown.ClearOptions();
        List<string> variableNames = new List<string>();
        foreach (var item in VariableController.instance.Dict)
        {
            variableNames.Add(item.key);
        }
        variableDropdown.AddOptions(variableNames);
    }

    private void PopulateOperationDropdown()
    {
        operationDropdown.ClearOptions();
        operationDropdown.AddOptions(new List<string> { "Definir", "Somar", "Subtrair" });
    }

    private void OnVariableChange(int index)
    {
        string selectedVariable = variableDropdown.options[index].text;
        ((SetVariableAction)action).VariableName = selectedVariable;
    }

    private void OnOperationChange(int index)
    {
        ((SetVariableAction)action).Operation = (VariableOperation)index;
    }

    private void OnValueChange(string newValue)
    {
        if (int.TryParse(newValue, out int valueAsInt))
        {
            ((SetVariableAction)action).VariableValue = valueAsInt;
        }
        else
        {
            Debug.LogWarning("Invalid input for variable value.");
            valueInputField.text = "";
        }
    }

    public override void SetAction(BaseAction action)
    {
        base.SetAction(action);
        SetVariableAction setAction = action as SetVariableAction;
        var optData = variableDropdown.options.FindIndex(opt => opt.Equals(setAction.VariableName));
        variableDropdown.value = optData;
        valueInputField.text = setAction.VariableValue.ToString();
        operationDropdown.value = (int)setAction.Operation;
    }
}
