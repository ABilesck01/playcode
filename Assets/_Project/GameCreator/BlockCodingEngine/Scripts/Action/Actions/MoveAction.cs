using UnityEngine;
using System;

[System.Serializable]
public class MoveAction : BaseAction
{
    [SerializeField] private Vector3 direction;

    public Vector3 Direction
    {
        get { return direction; }
        set { direction = value; }
    }

    public MoveAction()
    {
        id = Guid.NewGuid();
        this.Name = "Mover";
        this.TutorialText = "Move o evento na direção desejada";
    }

    public override void Execute(EventBlock block)
    {
        Transform objTransform = block.GetComponent<Transform>();
        if (objTransform != null)
        {
            objTransform.position += direction.normalized;
        }
    }
}