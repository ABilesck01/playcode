using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCamera : MonoBehaviour
{
    [SerializeField] private float panSpeed = 20f;
    [SerializeField] private float panBorderThickness = 10f;
    [SerializeField] private float panHeaderThickness = 150f;
    [Header("Limits")]
    [SerializeField] private Vector2 xLimit;
    [SerializeField] private Vector2 yLimit;

    private Transform myTransform;
    private bool use = true;

    private void Awake()
    {
        myTransform = transform;
    }

    private void OnEnable()
    {
        LevelController.OnStartLevel += LevelController_OnStartLevel;
        LevelController.OnStopLevel += LevelController_OnStopLevel;
    }

    private void OnDisable()
    {
        LevelController.OnStartLevel -= LevelController_OnStartLevel;
        LevelController.OnStopLevel -= LevelController_OnStopLevel;
    }

    private void LevelController_OnStartLevel(object sender, System.EventArgs e)
    {
        use = false;
    }

    private void LevelController_OnStopLevel(object sender, System.EventArgs e)
    {
        use = true;
    }

    private void Update()
    {
        if(!use) return;

        if (LevelController.hasOpenScreen) return;

        Vector3 pos = myTransform.position;

        if (Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - panHeaderThickness)
        {
            pos.y += panSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.S) || Input.mousePosition.y <= panBorderThickness)
        {
            pos.y -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= panBorderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }

        pos.x = Mathf.Clamp(pos.x, xLimit.x, xLimit.y);
        pos.y = Mathf.Clamp(pos.y, yLimit.x, yLimit.y);

        myTransform.position = pos;
    }
}
