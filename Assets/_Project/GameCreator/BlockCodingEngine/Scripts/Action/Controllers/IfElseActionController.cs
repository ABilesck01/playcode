using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class IfElseActionController : BaseController
{
    public TMP_Dropdown dropdownLeftVariable;
    public TMP_Dropdown dropdownRightVariable;
    public TMP_InputField inputRightVariable;
    public TMP_Dropdown dropdownOperator;
    public Transform ifActionContainer;
    public Toggle toggleExplicitRightVar;

    public Button btnSetIfAction;
    public Button btnSetElseAction;
    [Space]
    [SerializeField] private ConditionalActionView actionView;
    [Space]
    [SerializeField] private BaseController textAction;
    [SerializeField] private BaseController variableAction;
    [SerializeField] private BaseController moveAction;
    [SerializeField] private BaseController conditionAction;
    [SerializeField] private BaseController disableAction;
    [SerializeField] private BaseController endGameAction;

    private void Awake()
    {
        PopulateVariableDropdowns();

        dropdownLeftVariable.onValueChanged.AddListener(OnLeftVariableChange);
        dropdownRightVariable.onValueChanged.AddListener(OnRightVariableChange);
        dropdownOperator.onValueChanged.AddListener(OnOperatorChange);
        toggleExplicitRightVar.onValueChanged.AddListener(SetExplicitVariable);
        inputRightVariable.onValueChanged.AddListener(OnInputRightVariableChange);
        btnSetIfAction.onClick.AddListener(SetIfAction);
        //btnSetElseAction.onClick.AddListener(SetElseAction);
    }

    private void OnInputRightVariableChange(string newValue)
    {
        if (int.TryParse(newValue, out int result))
        {
            ((IfElseAction)action).RightVariableValue = result;
        }
        else
        {
            inputRightVariable.text = "0";
            ((IfElseAction)action).RightVariableValue = 0;
        }
    }

    private void SetExplicitVariable(bool isExp)
    {
        ((IfElseAction)action).ExplicitRightVariable = isExp;

        inputRightVariable.gameObject.SetActive(isExp);
        dropdownRightVariable.gameObject.SetActive(!isExp);
    }

    private void PopulateVariableDropdowns()
    {
        List<string> variableNames = GetVariableNames();
        dropdownLeftVariable.ClearOptions();
        dropdownRightVariable.ClearOptions();

        dropdownLeftVariable.AddOptions(variableNames);
        dropdownRightVariable.AddOptions(variableNames);
    }

    private List<string> GetVariableNames()
    {
        List<string> variableNames = new List<string>();
        foreach (var item in VariableController.instance.Dict)
        {
            variableNames.Add(item.key);
        }
        return variableNames;
    }

    private void OnLeftVariableChange(int index)
    {
        ((IfElseAction)action).LeftVariable = dropdownLeftVariable.options[index].text;
    }

    private void OnRightVariableChange(int index)
    {
        ((IfElseAction)action).RightVariable = dropdownRightVariable.options[index].text;
    }

    private void OnOperatorChange(int newIndex)
    {
        ((IfElseAction)action).Comparison = (IfElseAction.ComparisonOperator)newIndex;
    }

    private void SetIfAction()
    {
        //ActionView actionView = this.view;

        var canvas = FindObjectOfType<Canvas>();

        ActionView.OpenActionView(actionView, canvas.transform);
        ActionView.currentActionView.SetEvent(this.eventBlock);
        ActionView.currentActionView.OnAddAction += OnAddAction;
        //actionView.gameObject.SetActive(true);
    }

    private void OnAddAction(BaseController controller, BaseAction action)
    {
        btnSetIfAction.gameObject.SetActive(false);
        var ctl = Instantiate(controller, ifActionContainer);
        ctl.SetAction(action);
        ((IfElseAction)this.action).IfAction = action;
    }

    public override void SetAction(BaseAction action)
    {
        base.SetAction(action);

        if (dropdownLeftVariable.options.Count > 0)
        {
            OnLeftVariableChange(0);
            OnRightVariableChange(0);
        }

        IfElseAction ifElseAction = action as IfElseAction;
        if (ifElseAction != null)
        {
            int leftIndex = VariableController.instance.Dict.FindIndex(v => v.key == ifElseAction.LeftVariable);
            int rightIndex = VariableController.instance.Dict.FindIndex(v => v.key == ifElseAction.RightVariable);

            toggleExplicitRightVar.isOn = ifElseAction.ExplicitRightVariable;
            SetExplicitVariable(ifElseAction.ExplicitRightVariable);

            inputRightVariable.text = ifElseAction.RightVariableValue.ToString();

            dropdownLeftVariable.value = leftIndex >= 0 ? leftIndex : 0;
            dropdownRightVariable.value = rightIndex >= 0 ? rightIndex : 0;
            dropdownOperator.value = (int)ifElseAction.Comparison;

            if (ifElseAction.IfAction != null)
            {
                btnSetIfAction.gameObject.SetActive(false);
                InstantiateActionView(ifElseAction.IfAction, ifActionContainer);
            }
            if (ifElseAction.ElseAction != null)
            {
                InstantiateActionView(ifElseAction.ElseAction, ifActionContainer);
            }
        }
    }

    private void InstantiateActionView(BaseAction action, Transform container)
    {
        BaseController controller = InstantiateControllerForAction(action);
        controller.SetAction(action);
        controller.transform.SetParent(container, false);
    }

    private BaseController InstantiateControllerForAction(BaseAction action)
    {
        if(action is ShowTextAction)
        {
            return Instantiate(textAction, ifActionContainer);
        }
        else if (action is SetVariableAction)
        {
            return Instantiate(variableAction, ifActionContainer);
        }
        else if (action is MoveAction)
        {
            return Instantiate(moveAction, ifActionContainer);
        }
        else if (action is DisableObjectAction)
        {
            return Instantiate(disableAction, ifActionContainer);
        }
        else if (action is IfElseAction)
        {
            return Instantiate(conditionAction, ifActionContainer);
        }
        else if (action is EndGameAction)
        {
            return Instantiate(endGameAction, ifActionContainer);
        }
        else
        {
            throw new ArgumentException("No controller available for action type " + action.GetType().Name);
        }
    }
}
