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
        this.Name = "Display Text";
        this.TutorialText = "Use this command to display a text in view";
    }

    public override void Execute(EventBlock block)
    {
        Debug.Log("show text");
    }
}
