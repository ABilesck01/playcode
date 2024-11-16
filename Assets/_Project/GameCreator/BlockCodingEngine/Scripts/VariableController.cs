using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableController : MonoBehaviour
{
    [System.Serializable]
    public struct dictStruct
    {
        public string key;
        public int value;
    }
    private List<dictStruct> dict = new List<dictStruct>();
    private Dictionary<string, int> variables = new Dictionary<string, int>();

    public static VariableController instance;
    public static event Action onVariablesChanged;
    public List<dictStruct> Dict { get => dict; set => dict = value; }

    private void Awake()
    {
        instance = this;
        dict = new List<dictStruct>();
    }

    public void AddVariable(string name)
    {
        if (variables.ContainsKey(name))
        {
            return;
        }

        variables.Add(name, 0);
        dict.Add(new dictStruct { key = name, value = 0 });
        onVariablesChanged?.Invoke();
    }

    public void RemoveVariable(string name)
    {
        if (variables.Remove(name))
        {
            dict.RemoveAll(item => item.key == name);
            if (onVariablesChanged != null)
                onVariablesChanged.Invoke();
        }
    }

    public void SetVariable(string name, int value)
    {
        if(variables.ContainsKey(name))
        {
            variables[name] = value;
            var v = dict.Find(x => x.key == name);
            v.value = value;
        }
        else
        {
            variables.Add(name, value);
            dict.Add(new dictStruct { key = name, value = value });
        }
    }

    public int GetVariable(string name)
    {
        if (variables.ContainsKey(name))
        {
            return variables[name];
        }
        else
        {
            throw new KeyNotFoundException($"The variable '{name}' is not defined.");
        }
    }

    public void SetVariables(List<string> variableNames)
    {
        variables.Clear();
        foreach (var name in variableNames)
        {
            variables.Add(name, 0);
            dict.Add(new dictStruct { key = name, value = 0 });
        }
    }

    public List<Variavel> GetAllVariables()
    {
        List<Variavel> list = new List<Variavel>();

        foreach (var name in variables.Keys)
        {
            list.Add(new Variavel{
                nome = name,
            });
        }

        return list;
    }
}
