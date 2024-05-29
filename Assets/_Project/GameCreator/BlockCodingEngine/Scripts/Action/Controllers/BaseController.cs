using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    [SerializeField] protected BaseAction action;
    [SerializeField] protected ActionView view;
    [SerializeField] protected EventBlock eventBlock;

    public virtual void SetAction(BaseAction action)
    {
        this.action = action;
    }

    public void SetActionView(ActionView view)
    {
        this.view = view;
    }

    public void SetEventBlock(EventBlock eventBlockView)
    {
        this.eventBlock = eventBlockView;
    }
}
