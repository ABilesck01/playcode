using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionView : MonoBehaviour
{
    public static void OpenActionView(ActionView actionViewPrefab, Transform container)
    {
        currentActionView = Instantiate(actionViewPrefab, container);
        LevelController.hasOpenScreen = true;
        currentActionView.gameObject.SetActive(true);
    }

    public static ActionView currentActionView;

    [SerializeField] private Button btnClose;
    [SerializeField] protected Button BaseActionButtonTemplate;
    [SerializeField] protected Transform container;
    [Header("Action controller")]
    [SerializeField] protected BaseController textAction;
    [SerializeField] protected BaseController variableAction;
    [SerializeField] protected BaseController moveAction;
    [SerializeField] protected BaseController conditionAction;
    [SerializeField] protected BaseController disableAction;
    [SerializeField] protected BaseController endGameAction;

    public Action<BaseController, BaseAction> OnAddAction;

    private EventBlock eventBlock;

    private void Awake()
    {
        btnClose.onClick.AddListener(Close);

        AddActionToView<ShowTextAction>(textAction);
        AddActionToView<SetVariableAction>(variableAction);
        AddActionToView<MoveAction>(moveAction);
        AddActionToView<IfElseAction>(conditionAction);
        AddActionToView<DisableObjectAction>(disableAction);
        AddActionToView<EndGameAction>(endGameAction);
    }

    private void Close()
    {
        LevelController.hasOpenScreen = false;
        Destroy(currentActionView.gameObject);
        currentActionView = null;
    }

    public void SetEvent(EventBlock eventBlock)
    {
        this.eventBlock = eventBlock;
        OnAddAction = null;
    }

    public virtual void AddActionToView<T>(BaseController controller) where T : BaseAction, new()
    {
        T action = new T();
        var button = Instantiate(BaseActionButtonTemplate, container);
        button.GetComponentInChildren<TextMeshProUGUI>().text = action.Name;
        button.onClick.AddListener(() =>
        {
            eventBlock.EnqueueAction(action);
            Destroy(currentActionView.gameObject); 
            currentActionView = null;
            OnAddAction?.Invoke(controller, action);
        });
    }
}
