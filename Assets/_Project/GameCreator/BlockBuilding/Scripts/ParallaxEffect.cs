using System;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class ParallaxEffect : MonoBehaviour
{
    private float _startingPosX;
    private float _lengthOfSprite;
    public float AmountOfParallaxX;
    public Transform editorCamera;
    public Transform playCamera;

    private Transform currentCameraTransform;
    
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
        currentCameraTransform = playCamera;
    }

    private void LevelController_OnStopLevel(object sender, System.EventArgs e)
    {
        currentCameraTransform = editorCamera;
    }

    private void Start()
    {
        currentCameraTransform = editorCamera;

        // Getting the starting X position of sprite.
        _startingPosX = transform.position.x;
        // Getting the length of the sprite.
        _lengthOfSprite = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        Vector3 cameraPosition = currentCameraTransform.position;
        float tempX = cameraPosition.x * (1 - AmountOfParallaxX);
        float distanceX = cameraPosition.x * AmountOfParallaxX;

        Vector3 newPosition = new Vector3(_startingPosX + distanceX, transform.position.y, transform.position.z);
        newPosition.y = cameraPosition.y;

        transform.position = newPosition;

        // Only updating the X position based on the camera movement.
        if (tempX > _startingPosX + (_lengthOfSprite / 2))
        {
            _startingPosX += _lengthOfSprite;
        }
        else if (tempX < _startingPosX - (_lengthOfSprite / 2))
        {
            _startingPosX -= _lengthOfSprite;
        }
    }
}
