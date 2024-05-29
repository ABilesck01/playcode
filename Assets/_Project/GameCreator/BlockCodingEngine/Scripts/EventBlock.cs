using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

public class EventBlock : BaseBlock
{
    //comandos do evento
    //grafico
    [SerializeField] private EventGraphicsAsset tiles;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private int spriteIndex;
    private EventTrigger eventTrigger;

    private List<BaseAction> staticActionQueue;
    private Queue<BaseAction> actionQueue;
    private bool execute = false;

    private Sprite defaultSprite;
    private Vector3 defaultPosition;

    public List<EventGraphicsAsset.GraphicData> GetGraphics => tiles.allGraphics;
    public int CurrentSprite => spriteIndex + 1;
    public int CurrentTrigger => (int)eventTrigger;

    public List<BaseAction> GetActions => staticActionQueue;

    public static event EventHandler<EventBlock> OnActionAdded;

    [ContextMenu("Test serialization")]
    public string GetData()
    {
        EventBlockData data = new EventBlockData(this);
        string json = JsonConvert.SerializeObject(data);
        Debug.Log(json);
        return json;
    }

    public void DeserializeFromJson(string json)
    {
        EventBlockData data = JsonConvert.DeserializeObject<EventBlockData>(json);

        SetSpriteIndex(data.spriteIndex);
        SetTrigger(data.eventTrigger);
        actionQueue.Clear();
        staticActionQueue.Clear();

        foreach (var actionData in data.actions)
        {
            BaseAction action = DeserializeAction(actionData);
            EnqueueAction(action);
        }
    }

    private BaseAction DeserializeAction(ActionData data)
    {
        BaseAction action = null;
        switch (data.type)
        {
            case "MoveAction":
                action = new MoveAction
                {
                    Direction = Vector3FromString(data.parameters["Direction"].ToString())
                };
                break;
            case "SetVariableAction":
                action = new SetVariableAction
                {
                    VariableName = data.parameters["VariableName"].ToString(),
                    VariableValue = int.Parse(data.parameters["VariableValue"].ToString()),
                    Operation = (VariableOperation)Enum.Parse(typeof(VariableOperation), data.parameters["Operation"].ToString())
                };
                break;
            case "IfElseAction":
                action = new IfElseAction
                {
                    LeftVariable = data.parameters["LeftVariable"].ToString(),
                    RightVariable = data.parameters["RightVariable"].ToString(),
                    Comparison = (IfElseAction.ComparisonOperator)Enum.Parse(typeof(IfElseAction.ComparisonOperator), data.parameters["ComparisonOperator"].ToString())
                };
                break;
        }
        return action;
    }

    private Vector3 Vector3FromString(string vectorString)
    {
        string[] s = vectorString.Trim(new char[] { '(', ')' }).Split(',');
        return new Vector3(
            float.Parse(s[0]),
            float.Parse(s[1]),
            float.Parse(s[2]));
    }

    private void Awake()
    {
        defaultPosition = transform.position;
        spriteIndex = -1;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        defaultSprite = spriteRenderer.sprite;
        boxCollider = GetComponent<BoxCollider2D>();
        actionQueue = new Queue<BaseAction>();
        staticActionQueue = new List<BaseAction>();
    }

    private void OnEnable()
    {
        LevelController.OnStartLevel += LevelController_OnStartLevel;
        LevelController.OnStopLevel += LevelController_OnStopLevel;
        TickSystem.OnTick += TickSystem_OnTick;
    }

    private void OnDisable()
    {
        LevelController.OnStartLevel -= LevelController_OnStartLevel;
        LevelController.OnStopLevel -= LevelController_OnStopLevel;
        TickSystem.OnTick -= TickSystem_OnTick;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && eventTrigger == EventTrigger.OnPlayerTouch)
        { 
            execute = true; 
        }
    }

    private void TickSystem_OnTick(object sender, TickSystem.OnTickEventArgs e)
    {
        if (!execute)
            return;

        if(actionQueue.Count <= 0 )
        {
            return;
        }
        Debug.Log("executando", this);

        var action = actionQueue.Dequeue();
        action.Execute(this);
    }

    private void LevelController_OnStartLevel(object sender, System.EventArgs e)
    {
        if(eventTrigger == EventTrigger.OnPlayerTouch)
        {
            boxCollider.enabled = true;
            boxCollider.isTrigger = true;
        }
        else
        {
            boxCollider.enabled = false;
        }
        if(spriteIndex < 0)
            spriteRenderer.gameObject.SetActive(false);


        if(eventTrigger == EventTrigger.OnStartGame)
        {
            execute = true;
        }
    }

    private void LevelController_OnStopLevel(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);

        foreach(var action in staticActionQueue)
        {
            actionQueue.Enqueue(action);
        }
        execute = false;
        spriteRenderer.gameObject.SetActive(true);
        boxCollider.enabled = true;
        if (eventTrigger == EventTrigger.OnPlayerTouch)
        {
            boxCollider.isTrigger = false;
        }
        transform.position = defaultPosition;
    }

    public void SetSpriteIndex(int index)
    {
        spriteIndex = index;
        if (spriteIndex < 0)
        {
            spriteRenderer.sprite = defaultSprite;
            return;
        }
        spriteRenderer.sprite = tiles.allGraphics[spriteIndex].gfx;
    }

    public void SetTrigger(int index)
    {
        eventTrigger = (EventTrigger)index;
    }

    [ContextMenu("Open View")]
    public void OpenView()
    {
        EventBlockView.instance.OpenView(this);
    }

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0) && !LevelController.hasOpenScreen)
        {
            OpenView();
        }
    }

    public void EnqueueAction(BaseAction action)
    {
        if (actionQueue == null)
        {
            actionQueue = new Queue<BaseAction>();
        }

        Debug.Log($"Enqueued action {action.Name}");

        this.actionQueue.Enqueue(action);
        staticActionQueue.Add(action);

        OnActionAdded?.Invoke(this, this);
    }

    internal void StopExecution()
    {
        execute = false;
    }
}
public enum EventTrigger
{
    OnStartGame = 0,
    OnPlayerTouch,
    OnPlayerAction
}

