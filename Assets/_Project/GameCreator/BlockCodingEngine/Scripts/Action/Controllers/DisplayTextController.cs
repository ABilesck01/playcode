using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayTextController : BaseController
{
    [SerializeField] private TMP_InputField textToDisplay;

    private void Awake()
    {
        textToDisplay.onValueChanged.AddListener(OnTextChange);
    }

    private void OnTextChange(string newText)
    {
        ((ShowTextAction)action).Display = newText;
    }
}
