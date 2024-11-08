        using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginController : MonoBehaviour
{
    [Header("Input fields")]
    public TMP_InputField txtEmail;
    public TMP_InputField txtPassword;
    [Header("Buttons")]
    public Button btnSwitchToCreate;
    public Button btnSendLogin;
    [Header("Switch To Register")]
    public GameObject login;
    public GameObject register;

    private void Awake()
    {
        btnSwitchToCreate.onClick.AddListener(SwitchToRegister);
        btnSendLogin.onClick.AddListener(CreateAcount);
    }

    private void SwitchToRegister()
    {
        register.SetActive(true);
        login.SetActive(false);
    }

    private void CreateAcount()
    {
        btnSendLogin.interactable = false;
        ApiController.instance.SendRequest<LoginResponse>(RequestType.POST, "Usuario/login", OnSuccess, OnError, new LoginDto {email = txtEmail.text, senha = txtPassword.text });
    }

    private void OnError(string obj)
    {
        btnSendLogin.interactable = true;
        Debug.Log(obj);
        MessageBoxController.instance.ShowMessage("Erro", obj);
    }

    private void OnSuccess(LoginResponse response)
    {
        Debug.Log(response.ToString());
        PersistentGameData.usuario = response.loggedUser;
        SceneManager.LoadScene("Main");
    }

    private void ClearScreen()
    {
        txtEmail.text = string.Empty;
        txtPassword.text = string.Empty;
    }
}

public class LoginDto
{
    public string email;
    public string senha;
}

[System.Serializable]
public class Usuario
{
    public int ID;
    public string Nome;
    public string Email;
    public string Senha;
    public int Avatar;
    public List<int> Avatares;
    public int Trofeus;
    public int Moedas;
    public DateTime DataNascimento;

    public override string ToString()
    {
        return JsonUtility.ToJson(this);
    }
}

public class LoginResponse
{
    public string message;
    public Usuario loggedUser;

    public override string ToString()
    {
        return JsonUtility.ToJson(this);
    }
}

//{
//    "message": "Login successful",
//  "user": {
//        "id": 1,
//    "nome": "string",
//    "email": "string",
//    "senha": "string",
//    "avatar": 0,
//    "dataNascimento": "2024-08-30T00:02:36.182",
//    "userLevels": []
//  }
//}