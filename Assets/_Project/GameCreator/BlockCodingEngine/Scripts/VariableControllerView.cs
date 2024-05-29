using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class VariableControllerView : MonoBehaviour
{
    [SerializeField] private TMP_InputField variableName;
    [SerializeField] private Button btnAddVariable;
    [SerializeField] private Button btnClose;
    public GameObject variableItemPrefab;
    public Transform variableListContainer;

    private void Awake()
    {
        btnAddVariable.onClick.AddListener(AddVariable);
        btnClose.onClick.AddListener(Close);
    }

    private void Close()
    {
        LevelController.hasOpenScreen = false;
        gameObject.SetActive(false);
    }

    private void AddVariable()
    {
        if(string.IsNullOrEmpty(variableName.text))
        {
            return;
        }
        VariableController.instance.AddVariable(variableName.text);
    }

    private void OnEnable()
    {
        LevelController.hasOpenScreen = true;
        PopulateList();
        VariableController.onVariablesChanged += PopulateList;
    }

    private void OnDisable()
    {
        VariableController.onVariablesChanged -= PopulateList;
    }

    void PopulateList()
    {
        // Clear existing items
        foreach (Transform child in variableListContainer)
        {
            Destroy(child.gameObject);
        }

        // Add new items
        foreach (var variable in VariableController.instance.Dict)
        {
            GameObject item = Instantiate(variableItemPrefab, variableListContainer);
            TextMeshProUGUI texts = item.GetComponentInChildren<TextMeshProUGUI>();
            texts.text = variable.key;
        }
    }
}
