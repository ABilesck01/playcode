using System;

[System.Serializable]
public class DisableObjectAction : BaseAction
{
    public DisableObjectAction()
    {
        id = Guid.NewGuid();
        this.Name = "Destruir objeto";
        this.TutorialText = "Tira o objeto de cena";
    }

    public override void Execute(EventBlock block)
    {
        block.gameObject.SetActive(false);
        block.StopExecution();
    }
}