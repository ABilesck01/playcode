using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseController : MonoBehaviour
{
    [SerializeField] protected BaseAction action;
    [SerializeField] protected ActionView view;
    [SerializeField] protected EventBlock eventBlock;
    [SerializeField] protected EventBlockView eventBlockView;
    [SerializeField] private Button btnRemove;

    public virtual void SetAction(BaseAction action)
    {
        this.action = action;
        btnRemove.onClick.AddListener(RemoveActionFromBlock);
    }

    public void SetActionView(ActionView view)
    {
        this.view = view;
    }

    public void SetEventBlock(EventBlock eventBlock)
    {
        this.eventBlock = eventBlock;
    }

    public void SetEventBlockView(EventBlockView view)
    {
        eventBlockView = view;
    }

    public void RemoveActionFromBlock()
    {
        this.eventBlock.RemoveAction(this.action.id);
        eventBlockView.UpdateView(eventBlock);
    }
}
