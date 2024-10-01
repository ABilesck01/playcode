using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayLevelController : LevelController
{
    private void OnEnable()
    {
        LevelDataController.OnLoadLevel += LevelDataController_OnLoadLevel;
    }

    private void OnDisable()
    {
        LevelDataController.OnLoadLevel -= LevelDataController_OnLoadLevel;
    }

    private void LevelDataController_OnLoadLevel(object sender, System.EventArgs e)
    {
        PlayScenario();
    }
}
