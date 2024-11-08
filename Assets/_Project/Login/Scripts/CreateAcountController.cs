using System;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateAcountController : MonoBehaviour
{
    [Header("Input fields")]
    public TMP_InputField txtName;
    public TMP_InputField txtEmail;
    public TMP_InputField txtPassword;
    public TMP_InputField txtRepeatPassword;
    public TMP_InputField txtBirthDate;
    [Header("Buttons")]
    public Button btnSwitchToLogin;
    public Button btnSendCreate;
    [Header("Switch To Login")]
    public GameObject login;
    public GameObject register;

    private void Awake()
    {
        //txtBirthDate.onValueChanged.AddListener(delegate { OnValueChangeEvent(); });

        btnSwitchToLogin.onClick.AddListener(SwitchToLogin);
        btnSendCreate.onClick.AddListener(CreateAcount);
    }

    private void OnValueChangeEvent()
    {
        if (string.IsNullOrEmpty(txtBirthDate.text))
        {
            txtBirthDate.text = string.Empty;
        }
        else
        {
            string input = txtBirthDate.text;
            string MatchPattern = @"^((\d{2}\.){0,4}(\d{1,2})?)$";
            string ReplacementPattern = "$1.$3";
            string ToReplacePattern = @"((\.?\d{2})+)(\d)";

            input = Regex.Replace(input, ToReplacePattern, ReplacementPattern);
            Match result = Regex.Match(input, MatchPattern);
            if (result.Success)
            {
                txtBirthDate.text = input;
                txtBirthDate.caretPosition++;
            }
        }
    }

    private void SwitchToLogin()
    {
        register.SetActive(false);
        login.SetActive(true);
    }

    private void CreateAcount()
    {
        CreateUserDTO createUser = new CreateUserDTO 
        { 
            nome = txtName.text, 
            email = txtEmail.text, 
            senha = txtPassword.text, 
            dataNascimento = DateTime.Parse(txtBirthDate.text) 
        };

        ApiController.instance.SendRequest<string>(RequestType.POST,"Usuario/Create", OnSuccess, OnError, createUser);
    }

    private void OnError(string obj)
    {
        Debug.Log(obj);
        MessageBoxController.instance.ShowMessage("Erro", obj);

    }

    private void OnSuccess(string dto)
    {
        Debug.Log(dto);
        MessageBoxController.instance.ShowMessage("", "Sua conta foi criada!", () =>
        {
            ClearScreen();
            SwitchToLogin();
        });
    }

    private void ClearScreen()
    {
        txtName.text = string.Empty;
        txtEmail.text = string.Empty;
        txtPassword.text = string.Empty;
        txtRepeatPassword.text = string.Empty;
        txtBirthDate.text = string.Empty;
    }
}

public class CreateUserDTO
{
    public string nome;
    public string email;
    public string senha;
    public DateTime dataNascimento;
}
