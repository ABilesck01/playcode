using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventBlockView : MonoBehaviour
{
    [SerializeField] private GameObject view;
    [SerializeField] private Button closeButton;
    [SerializeField] private TMP_Dropdown graphicDropdown;
    [SerializeField] private TMP_Dropdown triggerDropdown;
    [Space]
    [SerializeField] private Button newAction;
    [SerializeField] private Transform actionContainer;
    [SerializeField] private ActionView actionView;
    [Header("Action controller")]
    [SerializeField] private BaseController textAction;
    [SerializeField] private BaseController variableAction;
    [SerializeField] private BaseController moveAction;
    [SerializeField] private BaseController conditionAction;
    [SerializeField] private BaseController disableAction;
    [SerializeField] private BaseController endGameAction;

    private EventBlock currentEvent;
    public static EventBlockView instance;

    private void Awake()
    { 

        instance = this;
        closeButton.onClick.AddListener(CloseView);
        graphicDropdown.onValueChanged.AddListener(OnGraphicDropdownChanged);
        triggerDropdown.onValueChanged.AddListener(OnTriggerDropdownChanged);
        newAction.onClick.AddListener(() => 
        {
            ActionView.OpenActionView(actionView, transform);
            ActionView.currentActionView.SetEvent(currentEvent);
            ActionView.currentActionView.OnAddAction += OnAddAction;
            //actionView.gameObject.SetActive(true);
        });
    }

    private void OnAddAction(BaseController controller, BaseAction action)
    {
        var ctl = Instantiate(controller, actionContainer);
        ctl.SetActionView(actionView);
        ctl.SetEventBlock(this.currentEvent);
        ctl.SetAction(action);
    }

    public void OpenView(EventBlock eventBlock)
    {
        LevelController.hasOpenScreen = true;
        foreach (Transform block in actionContainer)
        {
            Destroy(block.gameObject);
        }
        currentEvent = eventBlock;
       
        foreach (BaseAction action in currentEvent.GetActions)
        {
            AddActionToUI(action);
        }
        view.SetActive(true);
        UpdateGraphicDropdown();
        graphicDropdown.value = currentEvent.CurrentSprite;
        triggerDropdown.value = currentEvent.CurrentTrigger;
    }

    private void AddActionToUI(BaseAction action)
    {
        BaseController controller = InstantiateControllerForAction(action);
        controller.SetAction(action);
        controller.SetActionView(actionView);
        controller.transform.SetParent(actionContainer, false);
    }

    private BaseController InstantiateControllerForAction(BaseAction action)
    {
        switch (action.GetType().Name)
        {
            case nameof(ShowTextAction):
                return Instantiate(textAction);
            case nameof(SetVariableAction):
                return Instantiate(variableAction);
            case nameof(MoveAction):
                return Instantiate(moveAction);
            case nameof(IfElseAction):
                return Instantiate(conditionAction);
            case nameof(DisableObjectAction):
                return Instantiate(disableAction);
            case nameof(EndGameAction): 
                return Instantiate(endGameAction);
            default:
                throw new InvalidOperationException("No controller available for action type " + action.GetType().Name);
        }
    }

    public void CloseView() 
    {
        this.currentEvent.SaveData();

        LevelController.hasOpenScreen = false;
        view.SetActive(false);
    }

    private void UpdateGraphicDropdown()
    {
        if (currentEvent == null) return;

        graphicDropdown.ClearOptions();
        TMP_Dropdown.OptionData defaulData = new TMP_Dropdown.OptionData();
        defaulData.text = "None";
        List< TMP_Dropdown.OptionData> allData = new List<TMP_Dropdown.OptionData> ();
        allData.Add (defaulData);
        foreach (EventGraphicsAsset.GraphicData item in currentEvent.GetGraphics)
        {
            allData.Add(new TMP_Dropdown.OptionData
            {
                text = item.name,
                image = item.gfx
            });
        }
        graphicDropdown.AddOptions(allData);
    }

    private void OnGraphicDropdownChanged(int index)
    {
        currentEvent.SetSpriteIndex(index - 1);
    }

    private void OnTriggerDropdownChanged(int index)
    {
        currentEvent.SetTrigger(index);
    }
}
