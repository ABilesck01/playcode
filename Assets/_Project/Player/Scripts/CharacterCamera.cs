using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;
    public Vector2 xLimit;
    public Vector2 yLimit;
    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = new Vector3(
                Mathf.Clamp(smoothedPosition.x, xLimit.x, xLimit.y),
                Mathf.Clamp(smoothedPosition.y, yLimit.x, yLimit.y),
                transform.position.z
            );
        }
    }
}
