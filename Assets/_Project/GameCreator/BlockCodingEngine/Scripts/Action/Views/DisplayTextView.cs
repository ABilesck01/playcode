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

    private bool isShowingText = false;

    private void Awake()
    {
        instance = this;

        btnContinue.onClick.AddListener(CloseView);
    }

    private void CloseView()
    {
        display.text = string.Empty;
        view.SetActive(false);
        isShowingText = false;
    }

    public void ShowText(string text, UnityAction onFinishDisplay = null)
    {
        if (isShowingText) return;

        btnContinue.onClick.RemoveAllListeners();

        isShowingText = true;
        display.text = text;
        view.SetActive(true);
        btnContinue.onClick.AddListener(CloseView);
        btnContinue.onClick.AddListener(onFinishDisplay);
        //OnTextShow?.Invoke(this, EventArgs.Empty);
    }
}
