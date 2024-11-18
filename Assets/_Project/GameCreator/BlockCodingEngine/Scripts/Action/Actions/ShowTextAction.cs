using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTextAction : BaseAction
{
    private string display;

    public string Display
    {
        get { return display; }
        set { display = value; }
    }

    public ShowTextAction()
    {
        id = Guid.NewGuid();
        this.Name = "Mostrar texto";
    }

    public override void Execute(EventBlock block)
    {
        block.PauseExecution();
        DisplayTextView.instance.ShowText(display, () =>
        {
            block.ContinueExecution();
        });
    }
}
