using System;
using UnityEngine;

[System.Serializable]
public class BaseAction
{
    [SerializeField] private string name;

    public Guid id;

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public BaseAction()
    {
        id = Guid.NewGuid();
    }
    

    public virtual void Execute(EventBlock block)
	{

	}
}
