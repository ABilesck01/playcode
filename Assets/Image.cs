using System;
using UnityEngine;
using UnityEngine.UI;

public class Image : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image image;
    [SerializeField] private string base64;

    private void Start()
    {
        try
        {
            byte[] imageBytes = Convert.FromBase64String(base64);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(imageBytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            image.sprite = sprite;
        }
        catch (Exception ex)
        {
            Debug.LogError("Error loading image from Base64: " + ex.Message);
        }
    }
}
