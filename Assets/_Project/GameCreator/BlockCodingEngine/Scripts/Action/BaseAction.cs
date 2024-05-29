using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseAction
{
    [SerializeField] private string name;
	[SerializeField] private string tutorialText;

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public string TutorialText
	{
		get { return tutorialText; }
		set { tutorialText = value; }
	}

    

    public virtual void Execute(EventBlock block)
	{

	}
}
