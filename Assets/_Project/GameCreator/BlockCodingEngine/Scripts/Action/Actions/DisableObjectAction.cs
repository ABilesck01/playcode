using System;

[System.Serializable]
public class DisableObjectAction : BaseAction
{
    public DisableObjectAction()
    {
        id = Guid.NewGuid();
        this.Name = "Destruir objeto";
    }

    public override void Execute(EventBlock block)
    {
        block.Disable();
        block.StopExecution();
    }
}