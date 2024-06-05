using System;
using System.Reflection;
using TMPro;
using UnityEngine;

public class MoveActionController : BaseController
{
    public TMP_Dropdown directionDropdown;

    private void Awake()
    {
        directionDropdown.onValueChanged.AddListener(OnDirectionChange);
    }

    private void OnDirectionChange(int index)
    {
        Vector3 direction = Vector3.zero;
        switch (index)
        {
            case 0: // Direita
                direction = new Vector3(1, 0, 0);
                break;
            case 1: // Esquerda
                direction = new Vector3(-1, 0, 0);
                break;
            case 2: // Cima
                direction = new Vector3(0, 1, 0);
                break;
            case 3: // Baixo
                direction = new Vector3(0, -1, 0);
                break;
        }

        ((MoveAction)action).Direction = direction;
    }

    public override void SetAction(BaseAction action)
    {
        base.SetAction(action);
        MoveAction moveAction = action as MoveAction;
        if (moveAction != null)
        {
            Vector3 direction = moveAction.Direction;
            if (direction == new Vector3(1, 0, 0))
                directionDropdown.value = 0;
            else if (direction == new Vector3(-1, 0, 0))
                directionDropdown.value = 1;
            else if (direction == new Vector3(0, 1, 0))
                directionDropdown.value = 2;
            else if (direction == new Vector3(0, -1, 0))
                directionDropdown.value = 3;
        }
    }
}
