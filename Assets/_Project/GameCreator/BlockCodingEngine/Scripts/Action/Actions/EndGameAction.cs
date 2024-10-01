using System;

public class EndGameAction : BaseAction
{
    public EndGameAction()
    {
        id = Guid.NewGuid();
        Name = "Fim de jogo";
    }

    public override void Execute(EventBlock block)
    {
        GameOverBlock.CallGameOver(false);
    }
}
