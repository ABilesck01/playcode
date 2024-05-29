using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameAction : BaseAction
{
    public override void Execute(EventBlock block)
    {
        LevelController.instance.StopScenario();
    }
}
