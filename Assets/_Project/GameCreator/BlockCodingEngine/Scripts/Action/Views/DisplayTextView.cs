using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DisplayTextView : MonoBehaviour
{
    [SerializeField] private GameObject view;
    [SerializeField] private TextMeshProUGUI display;
    [SerializeField] private Button btnContinue;

    public static DisplayTextView instance;

    public static event EventHandler OnTextShow;
    public static event EventHandler OnTextHide;

    private void Awake()
    {
        instance = this;

        btnContinue.onClick.AddListener(CloseView);
    }

    private void CloseView()
    {
        display.text = string.Empty;
        view.SetActive(false);
    }

    public void ShowText(string text, UnityAction onFinishDisplay = null)
    {
        display.text = text;
        view.SetActive(true);
        btnContinue.onClick.AddListener(onFinishDisplay);
        //OnTextShow?.Invoke(this, EventArgs.Empty);
    }
}
