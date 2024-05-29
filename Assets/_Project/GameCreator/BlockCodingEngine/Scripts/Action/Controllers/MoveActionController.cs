using System;
using TMPro;
using UnityEngine;

public class MoveActionController : BaseController
{
    public TMP_InputField txtDirectionX;
    public TMP_InputField txtDirectionY;

    private void Awake()
    {
        txtDirectionX.onValueChanged.AddListener(OnDirectionXChange);
        txtDirectionY.onValueChanged.AddListener(OnDirectionYChange);
    }

    private void OnDirectionXChange(string newValue)
    {
        Vector3 direction = ((MoveAction)action).Direction;
        direction.x = float.Parse(newValue);
        ((MoveAction)action).Direction = direction;
    }

    private void OnDirectionYChange(string newValue)
    {
        Vector3 direction = ((MoveAction)action).Direction;
        direction.y = float.Parse(newValue);
        ((MoveAction)action).Direction = direction;
    }

    public override void SetAction(BaseAction action)
    {
        base.SetAction(action);
        MoveAction moveAction = action as MoveAction;
        if (moveAction != null)
        {
            txtDirectionX.text = moveAction.Direction.x.ToString();
            txtDirectionY.text = moveAction.Direction.y.ToString();
        }
    }
}
